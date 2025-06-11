using System.ComponentModel.DataAnnotations.Schema;

namespace Eletronic_Api.Model
{
    public class AppUser
    {
        public int AppUserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Profile { get; set; }
        [NotMapped]
        public IFormFile? ProfileFile { get; set; }

    }
}
