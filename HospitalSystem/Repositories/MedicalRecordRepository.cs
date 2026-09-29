using HospitalSystem.Data;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Repositories
{
    public class MedicalRecordRepository : Repository<MedicalRecord>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<MedicalRecord> GetRecordsWithPatient()
        {
            return _context.MedicalRecords
                .Include(m => m.Patient)
                .OrderByDescending(m => m.VisitDate)
                .ToList();
        }
    }
}
