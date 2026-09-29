using HospitalSystem.Data;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;

namespace HospitalSystem.Repositories
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        public JobRepository(AppDbContext context) : base(context)
        {
        }

        // هل المسمى الوظيفي مرتبط بأطباء؟
        public bool IsInUse(int jobId)
        {
            return _context.Doctors.Any(d => d.JobId == jobId);
        }
    }
}
