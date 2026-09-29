using HospitalSystem.Dtos.HospitalDtos;
using HospitalSystem.Models;
using HospitalSystem.Services;
using HospitalSystem.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Controllers
{
    public class ClinicsController : Controller
    {
        private readonly IClinicService _clinicService;

        public ClinicsController(IClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        [HasPermission(AppPermissions.ClinicsView)]
        public IActionResult Index()
        {
            return View(_clinicService.GetAllClinics());
        }

        [HasPermission(AppPermissions.ClinicsManage)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.ClinicsManage)]
        public IActionResult Create(ClinicDto clinicDto)
        {
            if (!ModelState.IsValid)
                return View(clinicDto);

            _clinicService.CreateClinic(clinicDto);
            TempData["Success"] = "تمت إضافة العيادة بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HasPermission(AppPermissions.ClinicsManage)]
        public IActionResult Edit(int id)
        {
            var clinic = _clinicService.GetClinicById(id);
            if (clinic == null)
                return NotFound();

            return View(clinic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.ClinicsManage)]
        public IActionResult Edit(Clinic clinic)
        {
            if (!ModelState.IsValid)
                return View(clinic);

            _clinicService.UpdateClinic(clinic);
            TempData["Success"] = "تم تعديل العيادة بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HasPermission(AppPermissions.ClinicsManage)]
        public IActionResult Delete(int id)
        {
            var clinic = _clinicService.GetClinicById(id);
            if (clinic == null)
                return NotFound();

            return View(clinic);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.ClinicsManage)]
        public IActionResult DeleteConfirmed(int id)
        {
            _clinicService.DeleteClinic(id);
            TempData["Success"] = "تم حذف العيادة";
            return RedirectToAction(nameof(Index));
        }
    }
}
