using HospitalSystem.Models;

namespace HospitalSystem.Services.Base
{
    public interface IRoleService
    {
        IEnumerable<Role> GetRolesWithPermissions();
        Role? GetRoleById(int id);
        Role? GetRoleWithPermissions(int id);
        void CreateRole(Role role);
        void UpdateRoleName(Role role, string? name);
        void DeleteRole(Role role);
        void SaveRolePermissions(Role role, List<int> permissionIds);

        IEnumerable<Permission> GetAllPermissions();
        Permission? GetPermissionById(int id);
        void CreatePermission(Permission permission);
        void UpdatePermissionName(Permission permission, string? name);
        void DeletePermission(Permission permission);
    }
}
