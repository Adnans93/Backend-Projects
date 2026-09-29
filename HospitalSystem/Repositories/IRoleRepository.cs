using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;

namespace HospitalSystem.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        IEnumerable<Role> GetRolesWithPermissions();
        Role? GetRoleWithPermissions(int id);
    }
}
