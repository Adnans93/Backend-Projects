using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;
using HospitalSystem.Services.Base;

namespace HospitalSystem.Services
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicalRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<MedicalRecord> GetRecordsWithPatient()
        {
            return _unitOfWork.MedicalRecordRepo.GetRecordsWithPatient();
        }

        public MedicalRecord? GetRecordById(int id)
        {
            return _unitOfWork.MedicalRecordRepo.GetById(id);
        }

        public void CreateRecord(MedicalRecord record)
        {
            _unitOfWork.MedicalRecordRepo.Add(record);
            _unitOfWork.Save();
        }

        public void UpdateRecord(MedicalRecord record)
        {
            _unitOfWork.MedicalRecordRepo.Update(record);
            _unitOfWork.Save();
        }

        public void DeleteRecord(int id)
        {
            var record = _unitOfWork.MedicalRecordRepo.GetById(id);
            if (record != null)
            {
                _unitOfWork.MedicalRecordRepo.Delete(record);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return _unitOfWork.PatientRepo.GetAll();
        }
    }
}
