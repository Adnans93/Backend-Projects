using System.ComponentModel.DataAnnotations;
using System.Data;

namespace HospitalSystem.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الصلاحية مطلوب")]
        [Display(Name = "اسم الصلاحية")]
        public string? Name { get; set; }

        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}