using HospitalSystem.Dtos.HospitalDtos;
using HospitalSystem.Services;
using HospitalSystem.Services.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalSystem.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPermissionService _permissionService;

        // حالات الموعد
        public static readonly string[] Statuses = { "مؤكد", "قيد الانتظار", "مكتمل", "ملغي" };

        public AppointmentsController(IAppointmentService appointmentService, IPermissionService permissionService)
        {
            _appointmentService = appointmentService;
            _permissionService = permissionService;
        }

        private void FillDropdowns(int? doctorId = null, int? patientId = null, string? status = null)
        {
            ViewBag.Doctors = new SelectList(_appointmentService.GetAllDoctors(), "Id", "Name", doctorId);
            ViewBag.Patients = new SelectList(_appointmentService.GetAllPatients(), "Id", "Name", patientId);
            ViewBag.Statuses = new SelectList(Statuses, status);
        }

        // الطبيب بدون صلاحية "عرض كل المواعيد" يرى مواعيده فقط
        private bool OwnOnly() => !_permissionService.Has(AppPermissions.AppointmentsView);

        [HasPermission(AppPermissions.AppointmentsView, AppPermissions.AppointmentsViewOwn)]
        public IActionResult Index(string? status)
        {
            ViewBag.OwnOnly = OwnOnly();
            ViewBag.Status = status;
            return View(_appointmentService.GetAppointments(status, _permissionService.GetDoctorId(), OwnOnly()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.AppointmentsChangeStatus)]
        public IActionResult ChangeStatus(int id, string status)
        {
            if (!Statuses.Contains(status))
                return NotFound();

            if (!_appointmentService.ChangeStatus(id, status, _permissionService.GetDoctorId(), OwnOnly()))
                return Forbid();

            TempData["Success"] = $"تم تغيير حالة الموعد إلى: {status}";
            return RedirectToAction(nameof(Index));
        }

        [HasPermission(AppPermissions.AppointmentsCreate)]
        public IActionResult Create(int? patientId)
        {
            FillDropdowns(patientId: patientId);
            return View(new AppointmentDto { PatientId = patientId ?? 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.AppointmentsCreate)]
        public IActionResult Create(AppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns(appointmentDto.DoctorId, appointmentDto.PatientId, appointmentDto.Status);
                return View(appointmentDto);
            }

            _appointmentService.CreateAppointment(appointmentDto);
            TempData["Success"] = "تم حجز الموعد بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HasPermission(AppPermissions.AppointmentsEdit)]
        public IActionResult Edit(int id)
        {
            var appointmentDto = _appointmentService.GetAppointmentById(id);
            if (appointmentDto == null)
                return NotFound();

            FillDropdowns(appointmentDto.DoctorId, appointmentDto.PatientId, appointmentDto.Status);
            return View(appointmentDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.AppointmentsEdit)]
        public IActionResult Edit(AppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns(appointmentDto.DoctorId, appointmentDto.PatientId, appointmentDto.Status);
                return View(appointmentDto);
            }

            _appointmentService.UpdateAppointment(appointmentDto);
            TempData["Success"] = "تم تعديل الموعد بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.AppointmentsDelete)]
        public IActionResult Delete(int id)
        {
            _appointmentService.DeleteAppointment(id);
            TempData["Success"] = "تم حذف الموعد";
            return RedirectToAction(nameof(Index));
        }
    }
}
