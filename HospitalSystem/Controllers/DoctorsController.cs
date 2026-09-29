using HospitalSystem.Dtos.HospitalDtos;
using HospitalSystem.Models;
using HospitalSystem.Services;
using HospitalSystem.Services.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalSystem.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // القوائم المنسدلة (العيادات والمسميات الوظيفية)
        private void FillDropdowns(int? clinicId = null, int? jobId = null)
        {
            ViewBag.Clinics = new SelectList(_doctorService.GetAllClinics(), "Id", "Name", clinicId);
            ViewBag.Jobs = new SelectList(_doctorService.GetAllJobs(), "Id", "Name", jobId);
        }

        [HasPermission(AppPermissions.DoctorsView)]
        public IActionResult Index()
        {
            return View(_doctorService.GetDoctorsWithJobAndClinic());
        }

        [HasPermission(AppPermissions.DoctorsView)]
        public IActionResult Details(int id)
        {
            var doctor = _doctorService.GetDoctorDetails(id);
            if (doctor == null)
                return NotFound();

            return View(doctor);
        }

        [HasPermission(AppPermissions.DoctorsManage)]
        public IActionResult Create()
        {
            FillDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.DoctorsManage)]
        public IActionResult Create(CreateDoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns(doctorDto.ClinicId, doctorDto.JobId);
                return View(doctorDto);
            }

            _doctorService.CreateDoctor(doctorDto);
            TempData["Success"] = "تمت إضافة الطبيب بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HasPermission(AppPermissions.DoctorsManage)]
        public IActionResult Edit(int id)
        {
            var doctor = _doctorService.GetDoctorById(id);
            if (doctor == null)
                return NotFound();

            FillDropdowns(doctor.ClinicId, doctor.JobId);
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.DoctorsManage)]
        public IActionResult Edit(Doctor doctor, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns(doctor.ClinicId, doctor.JobId);
                return View(doctor);
            }

            _doctorService.UpdateDoctor(doctor, imageFile);
            TempData["Success"] = "تم تعديل بيانات الطبيب بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.DoctorsManage)]
        public IActionResult Delete(int id)
        {
            _doctorService.DeleteDoctor(id);
            TempData["Success"] = "تم حذف الطبيب";
            return RedirectToAction(nameof(Index));
        }
    }
}
