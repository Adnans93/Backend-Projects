using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المريض مطلوب")]
        [Display(Name = "اسم المريض")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Display(Name = "رقم الهاتف")]
        public string? Phone { get; set; }

        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<MedicalRecord>? MedicalRecords { get; set; }
    }
}