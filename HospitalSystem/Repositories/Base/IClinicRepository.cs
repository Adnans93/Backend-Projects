using HospitalSystem.Dtos.HospitalDtos;
using HospitalSystem.Models;

namespace HospitalSystem.Repositories.Base
{
    public interface IClinicRepository : IRepository<Clinic>
    {
        List<ClinicStat> GetDoctorsCount(int take);
    }
}
