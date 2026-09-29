using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http; // تأكد من إضافة هذا الـ using لكي يتعرف على IFormFile

namespace HospitalSystem.Dtos.UsersDtos
{
    public class CreateUserDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? Password { get; set; }
        public string? HashPassword { get; set; }

        public string? Username { get; set; }

        // الطبيب المرتبط بالحساب (اختياري)
        public int? DoctorId { get; set; }

        public IFormFile? image { get; set; }
    }
}