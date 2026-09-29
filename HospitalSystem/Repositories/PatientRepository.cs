using HospitalSystem.Data;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Repositories
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(AppDbContext context) : base(context)
        {
        }

        // البحث بالاسم أو رقم الهاتف
        public IEnumerable<Patient> Search(string? search)
        {
            var query = _context.Patients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name!.Contains(search) || p.Phone!.Contains(search));
            }

            return query.OrderByDescending(p => p.Id).ToList();
        }

        // ملف المريض: المواعيد (مع الطبيب) والسجلات الطبية
        public Patient? GetPatientWithHistory(int id)
        {
            return _context.Patients
                .Include(p => p.Appointments!).ThenInclude(a => a.Doctor)
                .Include(p => p.MedicalRecords)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Patient> GetRecent(int take)
        {
            return _context.Patients.OrderByDescending(p => p.Id).Take(take).ToList();
        }
    }
}
