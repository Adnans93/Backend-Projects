using HospitalSystem.Models;
using HospitalSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // جداول المستشفى والعيادات
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Job> Jobs { get; set; }

        // جداول إدارة المستخدمين، الأدوار، والصلاحيات
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<PermissionRoles> PermissionRoles { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // المواعيد: ربط الطبيب والمريض
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId);

            // المستخدمين <-> الأدوار عبر جدول RoleUsers (جدول ربط واحد فقط)
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithMany(u => u.Roles)
                .UsingEntity<RoleUser>(
                    j => j.HasOne(ru => ru.Users).WithMany().HasForeignKey(ru => ru.UserId),
                    j => j.HasOne(ru => ru.Roles).WithMany().HasForeignKey(ru => ru.RoleId),
                    j => j.HasKey(ru => new { ru.UserId, ru.RoleId }));

            // الأدوار <-> الصلاحيات عبر جدول PermissionRoles (جدول ربط واحد فقط)
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<PermissionRoles>(
                    j => j.HasOne(pr => pr.Permissions).WithMany().HasForeignKey(pr => pr.PermissionsId),
                    j => j.HasOne(pr => pr.Roles).WithMany().HasForeignKey(pr => pr.RolesId),
                    j => j.HasKey(pr => new { pr.PermissionsId, pr.RolesId }));

            // المسمى الوظيفي: لا يمكن حذفه وهو مرتبط بأطباء
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Job)
                .WithMany(j => j.Doctors)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            // عند حذف طبيب مرتبط بحساب مستخدم: يبقى الحساب ويُلغى الربط فقط
            modelBuilder.Entity<User>()
                .HasOne(u => u.Doctor)
                .WithMany()
                .HasForeignKey(u => u.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);

            SeedData(modelBuilder);
        }

        // البيانات الأساسية: الصلاحيات، الأدوار، ومستخدم الأدمن (Adnan / 12345)
        private static void SeedData(ModelBuilder modelBuilder)
        {
            // 1. الصلاحيات
            var permissions = AppPermissions.All.ToList();
            modelBuilder.Entity<Permission>().HasData(
                permissions.Select((name, index) => new { Id = index + 1, Name = name }));

            // 2. الأدوار
            modelBuilder.Entity<Role>().HasData(
                new { Id = 1, Name = AppPermissions.AdminRole },
                new { Id = 2, Name = AppPermissions.DoctorRole },
                new { Id = 3, Name = AppPermissions.ReceptionRole });

            // 3. صلاحيات كل دور (الأدمن له كل الصلاحيات)
            var rolePermissions = permissions.Select((name, index) => new { PermissionsId = index + 1, RolesId = 1 }).ToList();
            rolePermissions.AddRange(AppPermissions.DefaultRoles[AppPermissions.DoctorRole]
                .Select(name => new { PermissionsId = permissions.IndexOf(name) + 1, RolesId = 2 }));
            rolePermissions.AddRange(AppPermissions.DefaultRoles[AppPermissions.ReceptionRole]
                .Select(name => new { PermissionsId = permissions.IndexOf(name) + 1, RolesId = 3 }));
            modelBuilder.Entity<PermissionRoles>().HasData(rolePermissions);

            // 4. مستخدم الأدمن: كلمة المرور 12345 مشفرة بـ BCrypt
            modelBuilder.Entity<User>().HasData(new
            {
                Id = 1,
                UID = "7f3c2a9e-1b4d-4c8a-9e2f-5d6a7b8c9d01",
                Name = "Adnan",
                Username = "Adnan",
                HashPassword = "$2b$11$S1r20/ZVSRUhBkVXoAraE.2WFFbReNBYQPjU.wz59WOcyaZTVH4Hu"
            });

            modelBuilder.Entity<RoleUser>().HasData(new { UserId = 1, RoleId = 1 });
        }
    }
}
