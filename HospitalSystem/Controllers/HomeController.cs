using System.Diagnostics;
using HospitalSystem.Models;
using HospitalSystem.Services;
using HospitalSystem.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPermissionService _permissionService;

        public HomeController(IAppointmentService appointmentService, IPermissionService permissionService)
        {
            _appointmentService = appointmentService;
            _permissionService = permissionService;
        }

        // لوحة التحكم (الطبيب يرى مواعيده فقط)
        public IActionResult Index()
        {
            var ownOnly = !_permissionService.Has(AppPermissions.AppointmentsView);
            var model = _appointmentService.GetDashboard(_permissionService.GetDoctorId(), ownOnly);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
