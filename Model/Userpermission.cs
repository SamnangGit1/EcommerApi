using System.ComponentModel.DataAnnotations;

namespace Eletronic_Api.Model
{
    public class Userpermission
    {
        [Key]
        public int UserPermissionID { get; set; }
        public int UserID { get; set; }
        public string? PermissionName { get; set; }
        public User? Users { get; set; }
    }
}
