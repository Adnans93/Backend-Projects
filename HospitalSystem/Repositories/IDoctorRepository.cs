using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;

namespace HospitalSystem.Repositories
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        IEnumerable<Doctor> GetDoctorsWithJobAndClinic();
        Doctor? GetDoctorWithJobAndClinic(int id);
    }
}
