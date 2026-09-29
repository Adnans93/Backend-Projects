using HospitalSystem.Models;
using HospitalSystem.Services;
using HospitalSystem.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Controllers
{
    [HasPermission(AppPermissions.RolesManage)]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View(_roleService.GetRolesWithPermissions());
        }

        public IActionResult Create() => View(new Role());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Role role)
        {
            if (!ModelState.IsValid)
                return View(role);

            _roleService.CreateRole(role);
            TempData["Success"] = "تمت إضافة الدور بنجاح";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var role = _roleService.GetRoleById(id);
            if (role == null)
                return NotFound();

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Role role)
        {
            var existing = _roleService.GetRoleById(role.Id);
            if (existing == null)
                return NotFound();

            // اسم دور الأدمن ثابت لأن النظام يعتمد عليه
            if (existing.Name == AppPermissions.AdminRole && role.Name != AppPermissions.AdminRole)
                ModelState.AddModelError(nameof(role.Name), "لا يمكن تغيير اسم دور Admin");

            if (!ModelState.IsValid)
                return View(role);

            _roleService.UpdateRoleName(existing, role.Name);
            TempData["Success"] = "تم تعديل الدور بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var role = _roleService.GetRoleById(id);
            if (role == null)
                return NotFound();

            // حماية دور الأدمن من الحذف
            if (role.Name == AppPermissions.AdminRole)
            {
                TempData["Error"] = "لا يمكن حذف دور Admin";
                return RedirectToAction(nameof(Index));
            }

            _roleService.DeleteRole(role);
            TempData["Success"] = "تم حذف الدور";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AssignPermissions(int roleId)
        {
            var role = _roleService.GetRoleWithPermissions(roleId);
            if (role == null)
                return NotFound();

            ViewBag.AllPermissions = _roleService.GetAllPermissions().ToList();
            ViewBag.AssignedPermissions = role.Permissions.Select(p => p.Id).ToList();
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssignPermissions(int roleId, List<int> permissionIds)
        {
            var role = _roleService.GetRoleWithPermissions(roleId);
            if (role == null)
                return NotFound();

            _roleService.SaveRolePermissions(role, permissionIds);
            TempData["Success"] = "تم حفظ صلاحيات الدور";
            return RedirectToAction(nameof(Index));
        }
    }
}
