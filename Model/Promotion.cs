using System.ComponentModel.DataAnnotations;

namespace Eletronic_Api.Model
{
    public class Promotion
    {
        [Key]
        public int PromotionID { get; set; }

        [Required]
        public string?   PromotionName { get; set; }

        [Required]
        public string? DiscountType { get; set; }

        [Required]
        public decimal DiscountValue { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
        public ICollection<Item>? Items { get; set; }
    }
}
