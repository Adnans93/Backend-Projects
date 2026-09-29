using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HospitalSystem.Dtos;
using HospitalSystem.Dtos.UsersDtos;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;
using HospitalSystem.Services.Base;
using Microsoft.IdentityModel.Tokens;

namespace HospitalSystem.Services
{
    public class UserService : IUserService
    {
        // اسم الكوكي الذي يُحفظ فيه التوكن وأسماء الـ Claims داخله
        public const string CookieName = "AuthToken";
        public const string ClaimUserId = "sub";
        public const string ClaimUserName = "name";
        public const string ClaimFullName = "fullname";
        public const string ClaimRole = "role";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public UserService(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        private string UploadFile(IFormFile file, string? name)
        {
            string fileName = name + "_" + Guid.NewGuid().ToString()
                              + Path.GetExtension(file.FileName);

            string folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "Users"
            );

            // Create folder if it doesn't exist
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return "/images/Users/" + fileName;
        }

        // ===== تسجيل الدخول و JWT Token =====

        public string? Login(string username, string password)
        {
            var user = _unitOfWork.UserRepo.GetByUsername(username);

            // التحقق من كلمة المرور المشفرة (Hashing)
            if (user == null || string.IsNullOrEmpty(user.HashPassword)
                || !BCrypt.Net.BCrypt.Verify(password, user.HashPassword))
            {
                return null;
            }

            var roles = _unitOfWork.UserRepo.GetRoleNames(user.Id);
            return CreateToken(user, roles);
        }

        public DateTime GetTokenExpiry()
        {
            var hours = _config.GetValue<int?>("Jwt:ExpiryHours") ?? 8;
            return DateTime.UtcNow.AddHours(hours);
        }

        private string CreateToken(User user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimUserId, user.Id.ToString()),
                new Claim(ClaimUserName, user.Username ?? ""),
                new Claim(ClaimFullName, user.Name ?? user.Username ?? "")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimRole, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: GetTokenExpiry(),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ===== المستخدمين =====

        public IEnumerable<UsersDto> GetAllUsers()
        {
            var users = _unitOfWork.UserRepo.GetUsersWithRolesAndDoctor();

            var usersDto = users.Select(u => new UsersDto
            {
                Id = u.Id,
                UID = u.UID,
                Name = u.Name,
                Username = u.Username,
                Email = u.Email,
                ImageURL = u.ImageURL,
                DoctorName = u.Doctor?.Name,
                Roles = u.Roles.Select(r => r.Name!).ToList()
            });

            return usersDto;
        }

        public User? GetUserById(int id)
        {
            return _unitOfWork.UserRepo.GetById(id);
        }

        public User? GetUserByUid(string uid)
        {
            return _unitOfWork.UserRepo.GetByUid(uid);
        }

        public bool UsernameExists(string username, int? exceptUserId = null)
        {
            return _unitOfWork.UserRepo.UsernameExists(username, exceptUserId);
        }

        public void CreateUser(CreateUserDto userDto)
        {
            //Mapping (كلمة المرور تُحفظ مشفرة فقط)
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Username = userDto.Username,
                DoctorId = userDto.DoctorId,
                HashPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            if (userDto.image != null)
            {
                user.ImageURL = UploadFile(userDto.image, userDto.Name);
            }

            _unitOfWork.UserRepo.Add(user);
            _unitOfWork.Save();
        }

        public void UpdateUser(UpdateUserDto userDto)
        {
            var user = _unitOfWork.UserRepo.GetById(userDto.Id);
            if (user == null)
                return;

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Username = userDto.Username;
            user.DoctorId = userDto.DoctorId;

            // تغيير كلمة المرور فقط إذا كتب كلمة جديدة
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.HashPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            if (userDto.image != null)
            {
                user.ImageURL = UploadFile(userDto.image, userDto.Name);
            }

            _unitOfWork.Save();
        }

        public void DeleteUser(User user)
        {
            _unitOfWork.UserRepo.Delete(user);
            _unitOfWork.Save();
        }

        // ===== أدوار المستخدم =====

        public UserRolesVM? GetUserRoles(int userId)
        {
            var user = _unitOfWork.UserRepo.GetById(userId);
            if (user == null)
                return null;

            var userRoleIds = _unitOfWork.UserRepo.GetRoleIds(userId);

            return new UserRolesVM
            {
                UserId = user.Id,
                UserName = user.Name,
                Roles = _unitOfWork.RoleRepo.GetAll().Select(role => new RoleCheckVM
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = userRoleIds.Contains(role.Id)
                }).ToList()
            };
        }

        public void SaveUserRoles(UserRolesVM model)
        {
            var selectedRoleIds = model.Roles.Where(r => r.IsSelected).Select(r => r.RoleId);
            _unitOfWork.UserRepo.SetRoles(model.UserId, selectedRoleIds);
            _unitOfWork.Save();
        }

        // ===== ملفات المستخدم =====

        public List<UserFile> GetUserFiles(int userId)
        {
            return _unitOfWork.UserRepo.GetFiles(userId);
        }

        public UserFile? GetUserFile(int fileId)
        {
            return _unitOfWork.UserRepo.GetFile(fileId);
        }

        public void AddUserFile(UserFile userFile, IFormFile file)
        {
            userFile.FileURL = UploadFile(file, userFile.Name);
            _unitOfWork.UserRepo.AddFile(userFile);
            _unitOfWork.Save();
        }

        public void DeleteUserFile(UserFile file)
        {
            _unitOfWork.UserRepo.DeleteFile(file);
            _unitOfWork.Save();
        }

        public IEnumerable<Doctor> GetAllDoctors()
        {
            return _unitOfWork.DoctorRepo.GetAll().OrderBy(d => d.Name);
        }
    }
}
