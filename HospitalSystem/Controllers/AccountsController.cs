using HospitalSystem.Services;
using HospitalSystem.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IUserService _userService;

        public AccountsController(IUserService userService)
        {
            _userService = userService;
        }

        // 1. صفحة تسجيل الدخول
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // 2. تسجيل الدخول: التحقق وإصدار JWT Token
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Username = username;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "الرجاء إدخال اسم المستخدم وكلمة المرور");
                return View();
            }

            var token = _userService.Login(username, password);
            if (token == null)
            {
                ModelState.AddModelError("", "اسم المستخدم أو كلمة المرور غير صحيحة");
                return View();
            }

            // حفظ التوكن في كوكي آمن
            Response.Cookies.Append(UserService.CookieName, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Strict,
                Expires = _userService.GetTokenExpiry()
            });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // 3. صفحة عدم وجود صلاحية
        public IActionResult AccessDenied()
        {
            return View();
        }

        // 4. تسجيل الخروج: حذف التوكن
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(UserService.CookieName);
            return RedirectToAction(nameof(Login));
        }
    }
}
