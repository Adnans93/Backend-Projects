using HospitalSystem.Dtos.HospitalDtos;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;
using HospitalSystem.Services.Base;

namespace HospitalSystem.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClinicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Clinic> GetAllClinics()
        {
            return _unitOfWork.ClinicRepo.GetAll();
        }

        public Clinic? GetClinicById(int id)
        {
            return _unitOfWork.ClinicRepo.GetById(id);
        }

        public void CreateClinic(ClinicDto clinicDto)
        {
            //Mapping
            var clinic = new Clinic
            {
                Name = clinicDto.Name,
                Description = clinicDto.Description
            };

            _unitOfWork.ClinicRepo.Add(clinic);
            _unitOfWork.Save();
        }

        public void UpdateClinic(Clinic clinic)
        {
            _unitOfWork.ClinicRepo.Update(clinic);
            _unitOfWork.Save();
        }

        public void DeleteClinic(int id)
        {
            var clinic = _unitOfWork.ClinicRepo.GetById(id);
            if (clinic != null)
            {
                _unitOfWork.ClinicRepo.Delete(clinic);
                _unitOfWork.Save();
            }
        }
    }
}
