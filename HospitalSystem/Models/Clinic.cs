using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.Models
{
    public class Clinic
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم العيادة مطلوب")]
        [Display(Name = "اسم العيادة")]
        public string? Name { get; set; }

        [Display(Name = "وصف العيادة")]
        public string? Description { get; set; }

        // العلاقة مع الأطباء (بحبح لو حاب تربط العيادة بأطبائها)
        public ICollection<Doctor>? Doctors { get; set; }
    }
}