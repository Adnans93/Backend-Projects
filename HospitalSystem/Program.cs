using System.Text;
using HospitalSystem.Data;
using HospitalSystem.Repositories.Base;
using HospitalSystem.Services;
using HospitalSystem.Services.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 1. قاعدة البيانات (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. كل الصفحات تتطلب تسجيل دخول، ما عدا ما عليه [AllowAnonymous]
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

// 3. تسجيل الدخول باستخدام JWT Token محفوظ في كوكي (HttpOnly)
var jwt = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            NameClaimType = UserService.ClaimUserName,
            RoleClaimType = UserService.ClaimRole
        };

        options.Events = new JwtBearerEvents
        {
            // قراءة التوكن من الكوكي
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies[UserService.CookieName];
                return Task.CompletedTask;
            },
            // غير مسجل دخول: تحويل لصفحة تسجيل الدخول
            OnChallenge = context =>
            {
                context.HandleResponse();
                var returnUrl = Uri.EscapeDataString(context.Request.Path + context.Request.QueryString);
                context.Response.Redirect($"/Accounts/Login?ReturnUrl={returnUrl}");
                return Task.CompletedTask;
            },
            // لا يملك الصلاحية: صفحة عدم وجود صلاحية
            OnForbidden = context =>
            {
                context.Response.Redirect("/Accounts/AccessDenied");
                return Task.CompletedTask;
            }
        };
    });

// 4. UnitOfWork + الخدمات (Services)
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

var app = builder.Build();

// 5. تطبيق الـ Migrations تلقائياً (إنشاء قاعدة البيانات والجداول والبيانات الأساسية)
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// أول صفحة تفتح هي Accounts/Login: أي صفحة تتطلب تسجيل دخول، فالزائر يُحوَّل تلقائياً لصفحة الدخول
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
