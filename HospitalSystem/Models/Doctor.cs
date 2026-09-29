using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSystem.Models
{
    [Index(nameof(UID), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        public string UID { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "اسم الطبيب مطلوب")]
        [Display(Name = "اسم الطبيب")]
        public string? Name { get; set; }

        [Display(Name = "الصورة الشخصية")]
        public string? ImageURL { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress]
        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Display(Name = "رقم الهاتف")]
        public string? Phone { get; set; }

        // إذا كنت تبي تربط الطبيب بتخصص أو وظيفة طبية (مثل استشاري، أخصائي...)
        [ForeignKey(nameof(Job))]
        [Display(Name = "المسمى الوظيفي")]
        public int? JobId { get; set; }
        public Job? Job { get; set; }

        // العلاقة الأساسية مع العيادة في المستشفى
        [ForeignKey(nameof(Clinic))]
        [Display(Name = "العيادة")]
        public int ClinicId { get; set; }
        public Clinic? Clinic { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}