using HospitalSystem.Models;

namespace HospitalSystem.Repositories.Base
{
    public interface IRoleRepository : IRepository<Role>
    {
        IEnumerable<Role> GetRolesWithPermissions();
        Role? GetRoleWithPermissions(int id);
    }
}
