using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSystem.Models
{
    public class UserFile
    {
        [Key]
        public int Id { get; set; }

        public string UID { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "اسم الملف مطلوب")]
        [Display(Name = "اسم الملف")]
        public string? Name { get; set; }

        public string FileURL { get; set; } = "";

        [ForeignKey(nameof(Users))]
        public int UserId { get; set; }

        public User? Users { get; set; }
    }
}