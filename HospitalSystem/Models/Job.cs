using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        public string UID { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "اسم الوظيفة مطلوب")]
        [Display(Name = "اسم الوظيفة")]
        public string? Name { get; set; }

        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}