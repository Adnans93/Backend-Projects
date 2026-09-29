using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSystem.Models
{
    public class PermissionRoles
    {
        [ForeignKey("Permissions")]
        public int PermissionsId { get; set; }
        public Permission Permissions { get; set; } = null!;

        [ForeignKey("Roles")]
        public int RolesId { get; set; }
        public Role Roles { get; set; } = null!;
    }
}