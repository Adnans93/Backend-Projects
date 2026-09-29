using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Display(Name = "الطبيب المعالج")]
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        [Display(Name = "المريض")]
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        [Required]
        [Display(Name = "تاريخ الموعد")]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;

        [Display(Name = "الحالة")]
        public string Status { get; set; } = "مؤكد";
    }
}