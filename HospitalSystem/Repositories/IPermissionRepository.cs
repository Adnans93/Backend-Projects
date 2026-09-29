using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;

namespace HospitalSystem.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        List<string> GetUserPermissions(int userId);
        List<Permission> GetByIds(IEnumerable<int> ids);
    }
}
