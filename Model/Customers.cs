using System.ComponentModel.DataAnnotations;

namespace Eletronic_Api.Model
{
    public class Customers
    {
        [Key]
        public int CustomerID { get; set; }
        [Required]
        public string? CustomerName { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]

        public string? Address { get; set; }
        [Required]
        public string? AddressType { get; set; }
        [Required]
        public string? HouseNo { get; set; }
    }
}
