using HospitalSystem.Models;
using HospitalSystem.Services;
using HospitalSystem.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Controllers
{
    public class PatientsController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HasPermission(AppPermissions.PatientsView)]
        public IActionResult Index(string? search)
        {
            ViewBag.Search = search;
            return View(_patientService.SearchPatients(search));
        }

        [HasPermission(AppPermissions.PatientsView)]
        public IActionResult Details(int id)
        {
            var patient = _patientService.GetPatientWithHistory(id);
            if (patient == null)
                return NotFound();

            return View(patient);
        }

        [HasPermission(AppPermissions.PatientsCreate)]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.PatientsCreate)]
        public IActionResult Create(Patient patient)
        {
            if (!ModelState.IsValid)
                return View(patient);

            _patientService.CreatePatient(patient);
            TempData["Success"] = "تمت إضافة المريض بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HasPermission(AppPermissions.PatientsEdit)]
        public IActionResult Edit(int id)
        {
            var patient = _patientService.GetPatientById(id);
            if (patient == null)
                return NotFound();

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.PatientsEdit)]
        public IActionResult Edit(Patient patient)
        {
            if (!ModelState.IsValid)
                return View(patient);

            _patientService.UpdatePatient(patient);
            TempData["Success"] = "تم تعديل بيانات المريض بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.PatientsDelete)]
        public IActionResult Delete(int id)
        {
            _patientService.DeletePatient(id);
            TempData["Success"] = "تم حذف المريض";
            return RedirectToAction(nameof(Index));
        }
    }
}
