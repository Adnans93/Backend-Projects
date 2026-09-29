using HospitalSystem.Models;

namespace HospitalSystem.Dtos.HospitalDtos
{
    public class DashboardVM
    {
        public int DoctorsCount { get; set; }
        public int PatientsCount { get; set; }
        public int ClinicsCount { get; set; }
        public int AppointmentsTodayCount { get; set; }
        public int UpcomingCount { get; set; }
        public int MedicalRecordsCount { get; set; }

        // true إذا كان المستخدم طبيباً يرى مواعيده فقط
        public bool OwnAppointmentsOnly { get; set; }

        public Dictionary<string, int> StatusCounts { get; set; } = new Dictionary<string, int>();
        public List<string> WeekLabels { get; set; } = new List<string>();
        public List<int> WeekCounts { get; set; } = new List<int>();

        public List<Appointment> TodayAppointments { get; set; } = new List<Appointment>();
        public List<ClinicStat> DoctorsPerClinic { get; set; } = new List<ClinicStat>();
        public List<Patient> RecentPatients { get; set; } = new List<Patient>();
    }

    public class ClinicStat
    {
        public string Name { get; set; } = "";
        public int DoctorsCount { get; set; }
    }
}
