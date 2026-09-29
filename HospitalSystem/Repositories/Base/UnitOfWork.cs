using HospitalSystem.Data;

namespace HospitalSystem.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            ClinicRepo = new ClinicRepository(context);
            DoctorRepo = new DoctorRepository(context);
            JobRepo = new JobRepository(context);
            PatientRepo = new PatientRepository(context);
            AppointmentRepo = new AppointmentRepository(context);
            MedicalRecordRepo = new MedicalRecordRepository(context);
            UserRepo = new UserRepository(context);
            RoleRepo = new RoleRepository(context);
            PermissionRepo = new PermissionRepository(context);
        }

        public IClinicRepository ClinicRepo { get; }
        public IDoctorRepository DoctorRepo { get; }
        public IJobRepository JobRepo { get; }
        public IPatientRepository PatientRepo { get; }
        public IAppointmentRepository AppointmentRepo { get; }
        public IMedicalRecordRepository MedicalRecordRepo { get; }
        public IUserRepository UserRepo { get; }
        public IRoleRepository RoleRepo { get; }
        public IPermissionRepository PermissionRepo { get; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
