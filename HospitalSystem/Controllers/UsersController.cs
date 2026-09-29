using HospitalSystem.Dtos.UsersDtos;
using HospitalSystem.Models;
using HospitalSystem.Services;
using HospitalSystem.Services.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalSystem.Controllers
{
    [HasPermission(AppPermissions.UsersManage)]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        private void FillDoctors(int? selected = null)
        {
            ViewBag.Doctors = new SelectList(_userService.GetAllDoctors(), "Id", "Name", selected);
        }

        public IActionResult Index()
        {
            return View(_userService.GetAllUsers());
        }

        public IActionResult Create()
        {
            FillDoctors();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Username))
                ModelState.AddModelError(nameof(userDto.Username), "اسم المستخدم مطلوب");
            else if (_userService.UsernameExists(userDto.Username))
                ModelState.AddModelError(nameof(userDto.Username), "اسم المستخدم مستخدم مسبقاً");

            if (string.IsNullOrWhiteSpace(userDto.Password))
                ModelState.AddModelError(nameof(userDto.Password), "كلمة المرور مطلوبة");

            if (!ModelState.IsValid)
            {
                FillDoctors(userDto.DoctorId);
                return View(userDto);
            }

            _userService.CreateUser(userDto);
            TempData["Success"] = "تمت إضافة المستخدم بنجاح";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(string uid)
        {
            var user = _userService.GetUserByUid(uid);
            if (user == null)
                return NotFound();

            ViewBag.ImageURL = user.ImageURL;
            FillDoctors(user.DoctorId);

            // كلمة المرور لا تُعرض، اتركها فارغة إذا لا تريد تغييرها
            return View(new UpdateUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Username = user.Username,
                DoctorId = user.DoctorId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UpdateUserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Username))
                ModelState.AddModelError(nameof(userDto.Username), "اسم المستخدم مطلوب");
            else if (_userService.UsernameExists(userDto.Username, userDto.Id))
                ModelState.AddModelError(nameof(userDto.Username), "اسم المستخدم مستخدم مسبقاً");

            if (!ModelState.IsValid)
            {
                ViewBag.ImageURL = _userService.GetUserById(userDto.Id)?.ImageURL;
                FillDoctors(userDto.DoctorId);
                return View(userDto);
            }

            _userService.UpdateUser(userDto);
            TempData["Success"] = "تم تعديل المستخدم بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            // منع المستخدم من حذف حسابه الحالي
            if (user.Username == User.Identity?.Name)
            {
                TempData["Error"] = "لا يمكنك حذف حسابك الحالي";
                return RedirectToAction(nameof(Index));
            }

            _userService.DeleteUser(user);
            TempData["Success"] = "تم حذف المستخدم";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ManageRoles(int id)
        {
            var model = _userService.GetUserRoles(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManageRoles(UserRolesVM model)
        {
            if (_userService.GetUserById(model.UserId) == null)
                return NotFound();

            _userService.SaveUserRoles(model);
            TempData["Success"] = "تم حفظ أدوار المستخدم، ستُطبّق بعد تسجيل دخوله من جديد";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ManageFiles(int UserId)
        {
            var user = _userService.GetUserById(UserId);
            if (user == null)
                return NotFound();

            ViewBag.Files = _userService.GetUserFiles(UserId);
            ViewBag.userName = user.Name;
            return View(new UserFile { UserId = UserId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManageFiles(UserFile userFile, IFormFile? file)
        {
            // رابط الملف يُولَّد بعد الرفع وليس من الفورم
            ModelState.Remove(nameof(UserFile.FileURL));

            if (file == null)
                ModelState.AddModelError("", "الرجاء اختيار ملف");

            if (!ModelState.IsValid)
            {
                ViewBag.Files = _userService.GetUserFiles(userFile.UserId);
                ViewBag.userName = _userService.GetUserById(userFile.UserId)?.Name;
                return View(userFile);
            }

            _userService.AddUserFile(userFile, file!);
            TempData["Success"] = "تم رفع الملف بنجاح";
            return RedirectToAction(nameof(ManageFiles), new { UserId = userFile.UserId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFile(int id)
        {
            var file = _userService.GetUserFile(id);
            if (file == null)
                return NotFound();

            _userService.DeleteUserFile(file);
            TempData["Success"] = "تم حذف الملف";
            return RedirectToAction(nameof(ManageFiles), new { UserId = file.UserId });
        }
    }
}
