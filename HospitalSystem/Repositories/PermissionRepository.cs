using HospitalSystem.Data;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;

namespace HospitalSystem.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext context) : base(context)
        {
        }

        // كل صلاحيات المستخدم من خلال أدواره
        public List<string> GetUserPermissions(int userId)
        {
            return _context.RoleUsers
                .Where(ru => ru.UserId == userId)
                .SelectMany(ru => ru.Roles.Permissions)
                .Select(p => p.Name!)
                .Distinct()
                .ToList();
        }

        public List<Permission> GetByIds(IEnumerable<int> ids)
        {
            return _context.Permissions.Where(p => ids.Contains(p.Id)).ToList();
        }
    }
}
