namespace HospitalSystem.Services.Base
{
    // صلاحيات المستخدم الحالي
    public interface IPermissionService
    {
        bool Has(string permission);
        HashSet<string> GetPermissions();
        int? GetDoctorId();
    }
}
