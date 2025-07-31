using System.ComponentModel.DataAnnotations.Schema;

namespace Eletronic_Api.Model
{
    public class Staff
    {
        public int StaffID { get; set; }    
        public string? StaffName { get; set; }
        public string? Sex { get; set; }
        public string? Email { get; set; } //-> email ->
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Department { get; set; } 
        public decimal Salary { get; set; } 
        public DateTime HiredDate { get; set; } 
        public string? Profile { get; set; }
        [NotMapped]
        public IFormFile? ProfileFile { get; set; } 
        public bool IsActive { get; set; }
        public ICollection<Order>? orders { get; set; }
    }
}
