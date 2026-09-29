using HospitalSystem.Dtos.HospitalDtos;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;
using HospitalSystem.Services.Base;

namespace HospitalSystem.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string UploadImage(IFormFile image, string? name)
        {
            string fileName = name + "_" + Guid.NewGuid().ToString()
                              + Path.GetExtension(image.FileName);

            string folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "Doctors"
            );

            // Create folder if it doesn't exist
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return "/images/Doctors/" + fileName;
        }

        private static DoctorDto ToDto(Doctor d)
        {
            return new DoctorDto
            {
                Id = d.Id,
                UID = d.UID,
                Name = d.Name,
                ImageURL = d.ImageURL,
                Email = d.Email,
                Phone = d.Phone,
                JobName = d.Job?.Name,
                ClinicName = d.Clinic?.Name
            };
        }

        public IEnumerable<DoctorDto> GetDoctorsWithJobAndClinic()
        {
            var doctors = _unitOfWork.DoctorRepo.GetDoctorsWithJobAndClinic();
            return doctors.Select(ToDto);
        }

        public DoctorDto? GetDoctorDetails(int id)
        {
            var doctor = _unitOfWork.DoctorRepo.GetDoctorWithJobAndClinic(id);
            return doctor == null ? null : ToDto(doctor);
        }

        public Doctor? GetDoctorById(int id)
        {
            return _unitOfWork.DoctorRepo.GetById(id);
        }

        public void CreateDoctor(CreateDoctorDto doctorDto)
        {
            //Mapping
            var doctor = new Doctor();

            if (doctorDto.Image != null)
            {
                doctor.ImageURL = UploadImage(doctorDto.Image, doctorDto.Name);
            }

            doctor.Name = doctorDto.Name;
            doctor.Email = doctorDto.Email;
            doctor.Phone = doctorDto.Phone;
            doctor.ClinicId = doctorDto.ClinicId;
            doctor.JobId = doctorDto.JobId;

            _unitOfWork.DoctorRepo.Add(doctor);
            _unitOfWork.Save();
        }

        public void UpdateDoctor(Doctor doctor, IFormFile? image)
        {
            if (image != null)
            {
                doctor.ImageURL = UploadImage(image, doctor.Name);
            }

            _unitOfWork.DoctorRepo.Update(doctor);
            _unitOfWork.Save();
        }

        public void DeleteDoctor(int id)
        {
            var doctor = _unitOfWork.DoctorRepo.GetById(id);
            if (doctor != null)
            {
                _unitOfWork.DoctorRepo.Delete(doctor);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<Clinic> GetAllClinics()
        {
            return _unitOfWork.ClinicRepo.GetAll();
        }

        public IEnumerable<Job> GetAllJobs()
        {
            return _unitOfWork.JobRepo.GetAll();
        }

        public Job? GetJobById(int id)
        {
            return _unitOfWork.JobRepo.GetById(id);
        }

        public void CreateJob(Job job)
        {
            _unitOfWork.JobRepo.Add(job);
            _unitOfWork.Save();
        }

        public void UpdateJob(Job job)
        {
            _unitOfWork.JobRepo.Update(job);
            _unitOfWork.Save();
        }

        // لا يمكن حذف مسمى وظيفي مرتبط بأطباء
        public bool DeleteJob(int id)
        {
            if (_unitOfWork.JobRepo.IsInUse(id))
                return false;

            var job = _unitOfWork.JobRepo.GetById(id);
            if (job != null)
            {
                _unitOfWork.JobRepo.Delete(job);
                _unitOfWork.Save();
            }
            return true;
        }
    }
}
