using HospitalSystem.Data;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Role> GetRolesWithPermissions()
        {
            return _context.Roles.Include(r => r.Permissions).ToList();
        }

        public Role? GetRoleWithPermissions(int id)
        {
            return _context.Roles.Include(r => r.Permissions).FirstOrDefault(r => r.Id == id);
        }
    }
}
