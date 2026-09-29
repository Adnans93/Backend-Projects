using HospitalSystem.Models;

namespace HospitalSystem.Repositories.Base
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        IEnumerable<Doctor> GetDoctorsWithJobAndClinic();
        Doctor? GetDoctorWithJobAndClinic(int id);
    }
}
