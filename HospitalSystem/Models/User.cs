using HospitalSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UID { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? Password { get; set; }
        public string? HashPassword { get; set; }

        public string? Username { get; set; }

        public string? ImageURL { get; set; }

        // ربط حساب المستخدم بطبيب (لحسابات الأطباء فقط) حتى يرى مواعيده هو
        [ForeignKey(nameof(Doctor))]
        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }


        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}