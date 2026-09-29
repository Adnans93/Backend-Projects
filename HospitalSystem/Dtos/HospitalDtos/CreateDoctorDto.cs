using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.Dtos.HospitalDtos
{
    public class CreateDoctorDto
    {
        [Required(ErrorMessage = "اسم الطبيب مطلوب")]
        [Display(Name = "اسم الطبيب")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress]
        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Display(Name = "رقم الهاتف")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "يجب اختيار العيادة")]
        [Display(Name = "العيادة")]
        public int ClinicId { get; set; }

        [Display(Name = "المسمى الوظيفي")]
        public int? JobId { get; set; }

        [Display(Name = "الصورة الشخصية")]
        public IFormFile? Image { get; set; }
    }
}
