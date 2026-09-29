using HospitalSystem.Data;
using HospitalSystem.Dtos.HospitalDtos;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;

namespace HospitalSystem.Repositories
{
    public class ClinicRepository : Repository<Clinic>, IClinicRepository
    {
        public ClinicRepository(AppDbContext context) : base(context)
        {
        }

        // عدد الأطباء في كل عيادة (للوحة التحكم)
        public List<ClinicStat> GetDoctorsCount(int take)
        {
            return _context.Clinics
                .Select(c => new ClinicStat { Name = c.Name ?? "", DoctorsCount = c.Doctors!.Count() })
                .OrderByDescending(c => c.DoctorsCount)
                .Take(take)
                .ToList();
        }
    }
}
