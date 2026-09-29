using HospitalSystem.Models;

namespace HospitalSystem.Repositories.Base
{
    public interface IJobRepository : IRepository<Job>
    {
        bool IsInUse(int jobId);
    }
}
