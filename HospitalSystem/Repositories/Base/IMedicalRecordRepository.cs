using HospitalSystem.Models;

namespace HospitalSystem.Repositories.Base
{
    public interface IMedicalRecordRepository : IRepository<MedicalRecord>
    {
        IEnumerable<MedicalRecord> GetRecordsWithPatient();
    }
}
