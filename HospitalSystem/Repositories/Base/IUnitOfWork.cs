namespace HospitalSystem.Repositories.Base
{
    public interface IUnitOfWork
    {
        IClinicRepository ClinicRepo { get; }
        IDoctorRepository DoctorRepo { get; }
        IJobRepository JobRepo { get; }
        IPatientRepository PatientRepo { get; }
        IAppointmentRepository AppointmentRepo { get; }
        IMedicalRecordRepository MedicalRecordRepo { get; }
        IUserRepository UserRepo { get; }
        IRoleRepository RoleRepo { get; }
        IPermissionRepository PermissionRepo { get; }

        void Save();
    }
}
