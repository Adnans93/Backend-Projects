using HospitalSystem.Data;
using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Repositories
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Doctor> GetDoctorsWithJobAndClinic()
        {
            return _context.Doctors
                .Include(d => d.Job)
                .Include(d => d.Clinic)
                .OrderBy(d => d.Name)
                .ToList();
        }

        public Doctor? GetDoctorWithJobAndClinic(int id)
        {
            return _context.Doctors
                .Include(d => d.Job)
                .Include(d => d.Clinic)
                .FirstOrDefault(d => d.Id == id);
        }
    }
}
