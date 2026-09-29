using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }

        [Display(Name = "المريض")]
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        [Required]
        [Display(Name = "التشخيص الطبي")]
        public string? Diagnosis { get; set; }

        [Display(Name = "الأدوية الموصوفة")]
        public string? Prescriptions { get; set; }

        [Display(Name = "تاريخ الزيارة")]
        public DateTime VisitDate { get; set; } = DateTime.Now;
    }
}