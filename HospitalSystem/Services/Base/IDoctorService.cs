using HospitalSystem.Dtos.HospitalDtos;
using HospitalSystem.Models;

namespace HospitalSystem.Services.Base
{
    public interface IDoctorService
    {
        IEnumerable<DoctorDto> GetDoctorsWithJobAndClinic();
        DoctorDto? GetDoctorDetails(int id);
        Doctor? GetDoctorById(int id);
        void CreateDoctor(CreateDoctorDto doctorDto);
        void UpdateDoctor(Doctor doctor, IFormFile? image);
        void DeleteDoctor(int id);

        IEnumerable<Clinic> GetAllClinics();

        // المسميات الوظيفية
        IEnumerable<Job> GetAllJobs();
        Job? GetJobById(int id);
        void CreateJob(Job job);
        void UpdateJob(Job job);
        bool DeleteJob(int id);
    }
}
