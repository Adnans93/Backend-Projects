using HospitalSystem.Models;
using HospitalSystem.Services;
using HospitalSystem.Services.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalSystem.Controllers
{
    public class MedicalRecordsController : Controller
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordsController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        private void FillPatients(int? selected = null)
        {
            ViewBag.Patients = new SelectList(_medicalRecordService.GetAllPatients(), "Id", "Name", selected);
        }

        [HasPermission(AppPermissions.MedicalRecordsView)]
        public IActionResult Index()
        {
            return View(_medicalRecordService.GetRecordsWithPatient());
        }

        [HasPermission(AppPermissions.MedicalRecordsCreate)]
        public IActionResult Create(int? patientId)
        {
            FillPatients(patientId);
            return View(new MedicalRecord { PatientId = patientId ?? 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.MedicalRecordsCreate)]
        public IActionResult Create(MedicalRecord record)
        {
            if (!ModelState.IsValid)
            {
                FillPatients(record.PatientId);
                return View(record);
            }

            _medicalRecordService.CreateRecord(record);
            TempData["Success"] = "تمت إضافة السجل الطبي بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HasPermission(AppPermissions.MedicalRecordsEdit)]
        public IActionResult Edit(int id)
        {
            var record = _medicalRecordService.GetRecordById(id);
            if (record == null)
                return NotFound();

            FillPatients(record.PatientId);
            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.MedicalRecordsEdit)]
        public IActionResult Edit(MedicalRecord record)
        {
            if (!ModelState.IsValid)
            {
                FillPatients(record.PatientId);
                return View(record);
            }

            _medicalRecordService.UpdateRecord(record);
            TempData["Success"] = "تم تعديل السجل الطبي بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission(AppPermissions.MedicalRecordsDelete)]
        public IActionResult Delete(int id)
        {
            _medicalRecordService.DeleteRecord(id);
            TempData["Success"] = "تم حذف السجل الطبي";
            return RedirectToAction(nameof(Index));
        }
    }
}
