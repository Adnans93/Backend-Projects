using System.Globalization;
using HospitalSystem.Dtos.HospitalDtos;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;
using HospitalSystem.Services.Base;

namespace HospitalSystem.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<AppointmentDto> GetAppointments(string? status, int? doctorId, bool ownOnly)
        {
            var appointments = _unitOfWork.AppointmentRepo.GetAppointmentsWithDoctorAndPatient(status, doctorId, ownOnly);

            var appointmentsDto = appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor?.Name,
                PatientId = a.PatientId,
                PatientName = a.Patient?.Name,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status
            });

            return appointmentsDto;
        }

        public AppointmentDto? GetAppointmentById(int id)
        {
            var a = _unitOfWork.AppointmentRepo.GetById(id);
            if (a == null)
                return null;

            return new AppointmentDto
            {
                Id = a.Id,
                DoctorId = a.DoctorId,
                PatientId = a.PatientId,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status
            };
        }

        public void CreateAppointment(AppointmentDto appointmentDto)
        {
            //Mapping
            var appointment = new Appointment
            {
                DoctorId = appointmentDto.DoctorId,
                PatientId = appointmentDto.PatientId,
                AppointmentDate = appointmentDto.AppointmentDate,
                Status = appointmentDto.Status ?? "مؤكد"
            };

            _unitOfWork.AppointmentRepo.Add(appointment);
            _unitOfWork.Save();
        }

        public void UpdateAppointment(AppointmentDto appointmentDto)
        {
            var appointment = _unitOfWork.AppointmentRepo.GetById(appointmentDto.Id);
            if (appointment == null)
                return;

            appointment.DoctorId = appointmentDto.DoctorId;
            appointment.PatientId = appointmentDto.PatientId;
            appointment.AppointmentDate = appointmentDto.AppointmentDate;
            appointment.Status = appointmentDto.Status ?? "مؤكد";

            _unitOfWork.Save();
        }

        // تغيير حالة الموعد (الطبيب لا يغيّر إلا مواعيده)
        public bool ChangeStatus(int id, string status, int? doctorId, bool ownOnly)
        {
            var appointment = _unitOfWork.AppointmentRepo.GetById(id);
            if (appointment == null)
                return false;

            if (ownOnly && appointment.DoctorId != doctorId)
                return false;

            appointment.Status = status;
            _unitOfWork.Save();
            return true;
        }

        public void DeleteAppointment(int id)
        {
            var appointment = _unitOfWork.AppointmentRepo.GetById(id);
            if (appointment != null)
            {
                _unitOfWork.AppointmentRepo.Delete(appointment);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<Doctor> GetAllDoctors()
        {
            return _unitOfWork.DoctorRepo.GetAll();
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return _unitOfWork.PatientRepo.GetAll();
        }

        public DashboardVM GetDashboard(int? doctorId, bool ownOnly)
        {
            var today = DateTime.Today;
            var weekStart = today.AddDays(-6);
            var repo = _unitOfWork.AppointmentRepo;

            var model = new DashboardVM
            {
                DoctorsCount = _unitOfWork.DoctorRepo.Count(),
                PatientsCount = _unitOfWork.PatientRepo.Count(),
                ClinicsCount = _unitOfWork.ClinicRepo.Count(),
                MedicalRecordsCount = _unitOfWork.MedicalRecordRepo.Count(),
                AppointmentsTodayCount = repo.CountBetween(today, today.AddDays(1), doctorId, ownOnly),
                UpcomingCount = repo.CountUpcoming(doctorId, ownOnly),
                OwnAppointmentsOnly = ownOnly,
                StatusCounts = repo.GetStatusCounts(doctorId, ownOnly),
                TodayAppointments = repo.GetUpcoming(7, doctorId, ownOnly).ToList(),
                DoctorsPerClinic = _unitOfWork.ClinicRepo.GetDoctorsCount(6),
                RecentPatients = _unitOfWork.PatientRepo.GetRecent(5).ToList()
            };

            // المواعيد خلال آخر 7 أيام
            var dates = repo.GetDatesBetween(weekStart, today.AddDays(1), doctorId, ownOnly);
            var culture = new CultureInfo("ar-SA") { DateTimeFormat = { Calendar = new GregorianCalendar() } };
            for (int i = 0; i < 7; i++)
            {
                var day = weekStart.AddDays(i);
                model.WeekLabels.Add(day.ToString("ddd d/M", culture));
                model.WeekCounts.Add(dates.Count(d => d.Date == day));
            }

            return model;
        }
    }
}
