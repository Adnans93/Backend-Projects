using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;

namespace HospitalSystem.Repositories
{
    public interface IMedicalRecordRepository : IRepository<MedicalRecord>
    {
        IEnumerable<MedicalRecord> GetRecordsWithPatient();
    }
}
