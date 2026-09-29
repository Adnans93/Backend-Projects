using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;

namespace HospitalSystem.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        IEnumerable<Patient> Search(string? search);
        Patient? GetPatientWithHistory(int id);
        IEnumerable<Patient> GetRecent(int take);
    }
}
