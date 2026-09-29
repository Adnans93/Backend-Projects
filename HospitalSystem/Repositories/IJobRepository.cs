using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;

namespace HospitalSystem.Repositories
{
    public interface IJobRepository : IRepository<Job>
    {
        bool IsInUse(int jobId);
    }
}
