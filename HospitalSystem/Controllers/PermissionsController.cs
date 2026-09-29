using HospitalSystem.Models;
using HospitalSystem.Services;
using HospitalSystem.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Controllers
{
    [HasPermission(AppPermissions.RolesManage)]
    public class PermissionsController : Controller
    {
        private readonly IRoleService _roleService;

        public PermissionsController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View(_roleService.GetAllPermissions());
        }

        public IActionResult Create() => View(new Permission());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Permission permission)
        {
            if (!ModelState.IsValid)
                return View(permission);

            _roleService.CreatePermission(permission);
            TempData["Success"] = "تمت إضافة الصلاحية بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // صلاحيات النظام الأساسية لا تُعدّل ولا تُحذف
        public IActionResult Edit(int id)
        {
            var permission = _roleService.GetPermissionById(id);
            if (permission == null)
                return NotFound();

            if (AppPermissions.IsSystem(permission.Name))
            {
                TempData["Error"] = "صلاحيات النظام الأساسية لا يمكن تعديلها";
                return RedirectToAction(nameof(Index));
            }

            return View(permission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Permission permission)
        {
            var existing = _roleService.GetPermissionById(permission.Id);
            if (existing == null)
                return NotFound();

            if (AppPermissions.IsSystem(existing.Name))
            {
                TempData["Error"] = "صلاحيات النظام الأساسية لا يمكن تعديلها";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(permission);

            _roleService.UpdatePermissionName(existing, permission.Name);
            TempData["Success"] = "تم تعديل الصلاحية بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var permission = _roleService.GetPermissionById(id);
            if (permission == null)
                return NotFound();

            if (AppPermissions.IsSystem(permission.Name))
            {
                TempData["Error"] = "صلاحيات النظام الأساسية لا يمكن حذفها";
                return RedirectToAction(nameof(Index));
            }

            _roleService.DeletePermission(permission);
            TempData["Success"] = "تم حذف الصلاحية";
            return RedirectToAction(nameof(Index));
        }
    }
}
