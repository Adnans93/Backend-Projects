using HospitalSystem.Models;

namespace HospitalSystem.Services.Base
{
    public interface IMedicalRecordService
    {
        IEnumerable<MedicalRecord> GetRecordsWithPatient();
        MedicalRecord? GetRecordById(int id);
        void CreateRecord(MedicalRecord record);
        void UpdateRecord(MedicalRecord record);
        void DeleteRecord(int id);

        IEnumerable<Patient> GetAllPatients();
    }
}
