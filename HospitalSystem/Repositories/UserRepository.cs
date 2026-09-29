using HospitalSystem.Data;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<User> GetUsersWithRolesAndDoctor()
        {
            return _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Doctor)
                .ToList();
        }

        public User? GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public User? GetByUid(string uid)
        {
            return _context.Users.FirstOrDefault(u => u.UID == uid);
        }

        public bool UsernameExists(string username, int? exceptUserId = null)
        {
            return _context.Users.Any(u => u.Username == username && (exceptUserId == null || u.Id != exceptUserId));
        }

        public int? GetDoctorId(int userId)
        {
            return _context.Users.Where(u => u.Id == userId).Select(u => u.DoctorId).FirstOrDefault();
        }

        public List<int> GetRoleIds(int userId)
        {
            return _context.RoleUsers.Where(ru => ru.UserId == userId).Select(ru => ru.RoleId).ToList();
        }

        public List<string> GetRoleNames(int userId)
        {
            return _context.RoleUsers.Where(ru => ru.UserId == userId).Select(ru => ru.Roles.Name!).ToList();
        }

        public void SetRoles(int userId, IEnumerable<int> roleIds)
        {
            var oldRoles = _context.RoleUsers.Where(ru => ru.UserId == userId).ToList();
            _context.RoleUsers.RemoveRange(oldRoles);

            foreach (var roleId in roleIds.Distinct())
            {
                _context.RoleUsers.Add(new RoleUser { UserId = userId, RoleId = roleId });
            }
        }

        public List<UserFile> GetFiles(int userId)
        {
            return _context.UserFiles.Where(f => f.UserId == userId).ToList();
        }

        public UserFile? GetFile(int fileId)
        {
            return _context.UserFiles.Find(fileId);
        }

        public void AddFile(UserFile file)
        {
            _context.UserFiles.Add(file);
        }

        public void DeleteFile(UserFile file)
        {
            _context.UserFiles.Remove(file);
        }
    }
}
