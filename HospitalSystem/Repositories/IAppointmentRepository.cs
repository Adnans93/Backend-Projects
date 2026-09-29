using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;

namespace HospitalSystem.Repositories
{
    // ownOnly = true: مواعيد الطبيب doctorId فقط
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        IEnumerable<Appointment> GetAppointmentsWithDoctorAndPatient(string? status, int? doctorId, bool ownOnly);
        IEnumerable<Appointment> GetUpcoming(int take, int? doctorId, bool ownOnly);
        int CountBetween(DateTime from, DateTime to, int? doctorId, bool ownOnly);
        int CountUpcoming(int? doctorId, bool ownOnly);
        Dictionary<string, int> GetStatusCounts(int? doctorId, bool ownOnly);
        List<DateTime> GetDatesBetween(DateTime from, DateTime to, int? doctorId, bool ownOnly);
    }
}
