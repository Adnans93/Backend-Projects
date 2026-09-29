using HospitalSystem.Repositories.Base;
using HospitalSystem.Services.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HospitalSystem.Services
{
    // صلاحيات المستخدم الحالي (تُقرأ من قاعدة البيانات مرة واحدة لكل طلب)
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _http;

        private HashSet<string>? _permissions;

        public PermissionService(IUnitOfWork unitOfWork, IHttpContextAccessor http)
        {
            _unitOfWork = unitOfWork;
            _http = http;
        }

        private int? CurrentUserId
        {
            get
            {
                var value = _http.HttpContext?.User.FindFirst(UserService.ClaimUserId)?.Value;
                return int.TryParse(value, out var id) ? id : null;
            }
        }

        public HashSet<string> GetPermissions()
        {
            if (_permissions == null)
            {
                var userId = CurrentUserId;
                _permissions = userId == null
                    ? new HashSet<string>()
                    : new HashSet<string>(_unitOfWork.PermissionRepo.GetUserPermissions(userId.Value));
            }
            return _permissions;
        }

        public bool Has(string permission)
        {
            return GetPermissions().Contains(permission);
        }

        // رقم الطبيب المرتبط بحساب المستخدم الحالي (إن وجد)
        public int? GetDoctorId()
        {
            var userId = CurrentUserId;
            return userId == null ? null : _unitOfWork.UserRepo.GetDoctorId(userId.Value);
        }
    }

    // يُكتب فوق الكنترولر أو الأكشن: [HasPermission(AppPermissions.PatientsDelete)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class HasPermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _permissions;

        public HasPermissionAttribute(params string[] permissions)
        {
            _permissions = permissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // غير مسجل دخول
            if (context.HttpContext.User.Identity?.IsAuthenticated != true)
            {
                context.Result = new ChallengeResult();
                return;
            }

            // يكفي أن يملك المستخدم واحدة من الصلاحيات المطلوبة
            var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();
            if (!_permissions.Any(permissionService.Has))
            {
                context.Result = new ForbidResult();
            }
        }
    }

    public static class AppPermissions
    {
        public const string ClinicsView = "Clinics.View";
        public const string ClinicsManage = "Clinics.Manage";

        public const string DoctorsView = "Doctors.View";
        public const string DoctorsManage = "Doctors.Manage";
        public const string JobsManage = "Jobs.Manage";

        public const string PatientsView = "Patients.View";
        public const string PatientsCreate = "Patients.Create";
        public const string PatientsEdit = "Patients.Edit";
        public const string PatientsDelete = "Patients.Delete";

        public const string AppointmentsView = "Appointments.View";
        public const string AppointmentsViewOwn = "Appointments.ViewOwn";
        public const string AppointmentsCreate = "Appointments.Create";
        public const string AppointmentsEdit = "Appointments.Edit";
        public const string AppointmentsChangeStatus = "Appointments.ChangeStatus";
        public const string AppointmentsDelete = "Appointments.Delete";

        public const string MedicalRecordsView = "MedicalRecords.View";
        public const string MedicalRecordsCreate = "MedicalRecords.Create";
        public const string MedicalRecordsEdit = "MedicalRecords.Edit";
        public const string MedicalRecordsDelete = "MedicalRecords.Delete";

        public const string UsersManage = "Users.Manage";
        public const string RolesManage = "Roles.Manage";

        public static readonly IReadOnlyDictionary<string, string> DisplayNames = new Dictionary<string, string>
        {
            [ClinicsView] = "عرض العيادات",
            [ClinicsManage] = "إدارة العيادات (إضافة/تعديل/حذف)",
            [DoctorsView] = "عرض الأطباء",
            [DoctorsManage] = "إدارة الأطباء (إضافة/تعديل/حذف)",
            [JobsManage] = "إدارة المسميات الوظيفية",
            [PatientsView] = "عرض المرضى",
            [PatientsCreate] = "إضافة مريض",
            [PatientsEdit] = "تعديل مريض",
            [PatientsDelete] = "حذف مريض",
            [AppointmentsView] = "عرض كل المواعيد",
            [AppointmentsViewOwn] = "عرض مواعيد الطبيب نفسه فقط",
            [AppointmentsCreate] = "حجز موعد",
            [AppointmentsEdit] = "تعديل موعد",
            [AppointmentsChangeStatus] = "تغيير حالة الموعد (تأكيد/إكمال/إلغاء)",
            [AppointmentsDelete] = "حذف موعد",
            [MedicalRecordsView] = "عرض السجلات الطبية",
            [MedicalRecordsCreate] = "إضافة سجل طبي",
            [MedicalRecordsEdit] = "تعديل سجل طبي",
            [MedicalRecordsDelete] = "حذف سجل طبي",
            [UsersManage] = "إدارة المستخدمين",
            [RolesManage] = "إدارة الأدوار والصلاحيات",
        };

        public static IEnumerable<string> All => DisplayNames.Keys;

        public static string DisplayName(string? key) =>
            key != null && DisplayNames.TryGetValue(key, out var name) ? name : key ?? "";

        public static bool IsSystem(string? key) => key != null && DisplayNames.ContainsKey(key);

        // أسماء الأدوار الأساسية
        public const string AdminRole = "Admin";
        public const string DoctorRole = "Doctor";
        public const string ReceptionRole = "Reception";

        // الصلاحيات الافتراضية لأدوار الطبيب والاستقبال
        public static readonly IReadOnlyDictionary<string, string[]> DefaultRoles = new Dictionary<string, string[]>
        {
            [DoctorRole] = new[]
            {
                ClinicsView, DoctorsView, PatientsView,
                AppointmentsViewOwn, AppointmentsChangeStatus,
                MedicalRecordsView, MedicalRecordsCreate, MedicalRecordsEdit
            },
            [ReceptionRole] = new[]
            {
                ClinicsView, DoctorsView,
                PatientsView, PatientsCreate, PatientsEdit,
                AppointmentsView, AppointmentsCreate, AppointmentsEdit, AppointmentsChangeStatus
            },
        };
    }
}
