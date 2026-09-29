using HospitalSystem.Models;

namespace HospitalSystem.Repositories.Base
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        List<string> GetUserPermissions(int userId);
        List<Permission> GetByIds(IEnumerable<int> ids);
    }
}
