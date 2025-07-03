using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eletronic_Api.Model
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
      
        public string? Profile { get; set; }
        public bool IsAdmin { get; set; } = false;
        public DateTime UserTime { get; set; } = DateTime.Now;
        [NotMapped]
        public IFormFile? ProfileFile { get; set; }
        public ICollection<Userpermission>? UserPermissions { get; set; }

    }
}
