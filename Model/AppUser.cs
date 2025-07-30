using System.ComponentModel.DataAnnotations.Schema;

namespace Eletronic_Api.Model
{
    public class AppUser
    {
        public int AppUserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; } //-> email ->
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string ? AddressType { get; set; } //-> home, office, etc. ->
        public string? HouseNo { get; set; } //-> house number ->
        public string? Profile { get; set; }
        [NotMapped]
        public IFormFile? ProfileFile { get; set; }
     
        public bool IsVerified { get; set; }
        public ICollection<ItemCard>? ItemCards { get; set; }
    }
}
