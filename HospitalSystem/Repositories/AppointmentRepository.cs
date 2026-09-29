using HospitalSystem.Data;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context)
        {
        }

        private IQueryable<Appointment> Filter(int? doctorId, bool ownOnly)
        {
            var query = _context.Appointments.AsQueryable();
            if (ownOnly)
            {
                query = query.Where(a => doctorId != null && a.DoctorId == doctorId);
            }
            return query;
        }

        public IEnumerable<Appointment> GetAppointmentsWithDoctorAndPatient(string? status, int? doctorId, bool ownOnly)
        {
            var query = Filter(doctorId, ownOnly)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(a => a.Status == status);
            }

            return query.OrderByDescending(a => a.AppointmentDate).ToList();
        }

        public IEnumerable<Appointment> GetUpcoming(int take, int? doctorId, bool ownOnly)
        {
            return Filter(doctorId, ownOnly)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.AppointmentDate >= DateTime.Today)
                .OrderBy(a => a.AppointmentDate)
                .Take(take)
                .ToList();
        }

        public int CountBetween(DateTime from, DateTime to, int? doctorId, bool ownOnly)
        {
            return Filter(doctorId, ownOnly).Count(a => a.AppointmentDate >= from && a.AppointmentDate < to);
        }

        public int CountUpcoming(int? doctorId, bool ownOnly)
        {
            return Filter(doctorId, ownOnly).Count(a => a.AppointmentDate >= DateTime.Today);
        }

        public Dictionary<string, int> GetStatusCounts(int? doctorId, bool ownOnly)
        {
            return Filter(doctorId, ownOnly)
                .GroupBy(a => a.Status)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionary(x => x.Key, x => x.Count);
        }

        public List<DateTime> GetDatesBetween(DateTime from, DateTime to, int? doctorId, bool ownOnly)
        {
            return Filter(doctorId, ownOnly)
                .Where(a => a.AppointmentDate >= from && a.AppointmentDate < to)
                .Select(a => a.AppointmentDate)
                .ToList();
        }
    }
}
