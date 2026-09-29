using HospitalSystem.Dtos.HospitalDtos;
using HospitalSystem.Models;

namespace HospitalSystem.Services.Base
{
    public interface IClinicService
    {
        IEnumerable<Clinic> GetAllClinics();
        Clinic? GetClinicById(int id);
        void CreateClinic(ClinicDto clinicDto);
        void UpdateClinic(Clinic clinic);
        void DeleteClinic(int id);
    }
}
