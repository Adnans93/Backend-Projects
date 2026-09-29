using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HospitalSystem.Dtos.HospitalDtos
{
    public class ClinicDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم العيادة مطلوب")]
        [Display(Name = "اسم العيادة")]
        public string? Name { get; set; }

        [Display(Name = "وصف العيادة")]
        public string? Description { get; set; }

        [Display(Name = "صورة العيادة")]
        public string? ImageUrl { get; set; }

        [Display(Name = "رفع صورة العيادة")]
        public IFormFile? ImageFile { get; set; }
    }
}