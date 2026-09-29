using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;
using HospitalSystem.Services.Base;

namespace HospitalSystem.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ===== الأدوار =====

        public IEnumerable<Role> GetRolesWithPermissions()
        {
            return _unitOfWork.RoleRepo.GetRolesWithPermissions();
        }

        public Role? GetRoleById(int id)
        {
            return _unitOfWork.RoleRepo.GetById(id);
        }

        public Role? GetRoleWithPermissions(int id)
        {
            return _unitOfWork.RoleRepo.GetRoleWithPermissions(id);
        }

        public void CreateRole(Role role)
        {
            _unitOfWork.RoleRepo.Add(role);
            _unitOfWork.Save();
        }

        public void UpdateRoleName(Role role, string? name)
        {
            role.Name = name;
            _unitOfWork.Save();
        }

        public void DeleteRole(Role role)
        {
            _unitOfWork.RoleRepo.Delete(role);
            _unitOfWork.Save();
        }

        public void SaveRolePermissions(Role role, List<int> permissionIds)
        {
            role.Permissions.Clear();
            foreach (var permission in _unitOfWork.PermissionRepo.GetByIds(permissionIds))
            {
                role.Permissions.Add(permission);
            }
            _unitOfWork.Save();
        }

        // ===== الصلاحيات =====

        public IEnumerable<Permission> GetAllPermissions()
        {
            return _unitOfWork.PermissionRepo.GetAll();
        }

        public Permission? GetPermissionById(int id)
        {
            return _unitOfWork.PermissionRepo.GetById(id);
        }

        public void CreatePermission(Permission permission)
        {
            _unitOfWork.PermissionRepo.Add(permission);
            _unitOfWork.Save();
        }

        public void UpdatePermissionName(Permission permission, string? name)
        {
            permission.Name = name;
            _unitOfWork.Save();
        }

        public void DeletePermission(Permission permission)
        {
            _unitOfWork.PermissionRepo.Delete(permission);
            _unitOfWork.Save();
        }
    }
}
