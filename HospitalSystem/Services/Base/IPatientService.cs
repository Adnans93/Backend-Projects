using HospitalSystem.Models;

namespace HospitalSystem.Services.Base
{
    public interface IPatientService
    {
        IEnumerable<Patient> SearchPatients(string? search);
        Patient? GetPatientById(int id);
        Patient? GetPatientWithHistory(int id);
        void CreatePatient(Patient patient);
        void UpdatePatient(Patient patient);
        void DeletePatient(int id);
    }
}
