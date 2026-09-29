using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الدور مطلوب")]
        [Display(Name = "اسم الدور")]
        public string? Name { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}