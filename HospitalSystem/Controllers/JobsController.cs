using HospitalSystem.Models;
using HospitalSystem.Services;
using HospitalSystem.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Controllers
{
    // المسميات الوظيفية للأطباء (استشاري، أخصائي، طبيب مقيم...)
    [HasPermission(AppPermissions.JobsManage)]
    public class JobsController : Controller
    {
        private readonly IDoctorService _doctorService;

        public JobsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public IActionResult Index()
        {
            return View(_doctorService.GetAllJobs());
        }

        public IActionResult Create() => View(new Job());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Job job)
        {
            if (!ModelState.IsValid)
                return View(job);

            _doctorService.CreateJob(job);
            TempData["Success"] = "تمت إضافة المسمى الوظيفي";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var job = _doctorService.GetJobById(id);
            if (job == null)
                return NotFound();

            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Job job)
        {
            if (!ModelState.IsValid)
                return View(job);

            _doctorService.UpdateJob(job);
            TempData["Success"] = "تم تعديل المسمى الوظيفي";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (_doctorService.DeleteJob(id))
                TempData["Success"] = "تم حذف المسمى الوظيفي";
            else
                TempData["Error"] = "لا يمكن حذف هذا المسمى لأنه مرتبط بأطباء، عدّل الأطباء أولاً";

            return RedirectToAction(nameof(Index));
        }
    }
}
