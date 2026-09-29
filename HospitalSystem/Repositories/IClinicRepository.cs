using HospitalSystem.Dtos.HospitalDtos;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;

namespace HospitalSystem.Repositories
{
    public interface IClinicRepository : IRepository<Clinic>
    {
        List<ClinicStat> GetDoctorsCount(int take);
    }
}
