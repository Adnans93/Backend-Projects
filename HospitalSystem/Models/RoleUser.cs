using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSystem.Models
{
    public class RoleUser
    {
        [ForeignKey("Roles")]
        public int RoleId { get; set; }
        public Role Roles { get; set; } = null!;

        [ForeignKey("Users")]
        public int UserId { get; set; }
        public User Users { get; set; } = null!;
    }
}