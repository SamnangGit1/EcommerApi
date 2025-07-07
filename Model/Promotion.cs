using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eletronic_Api.Model
{
    public class Promotion
    {
        [Key]
        public int PromotionID { get; set; }
        public int TargetID { get; set; }

       // [Required]
        public string? PromotionType { get; set; }

        public string? PromotionName { get; set; }
        public string? Description { get; set; }
        public decimal DiscountPercents { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool AlertNotification { get; set; }

        [NotMapped]
        public Item? Item { get; set; }

        [NotMapped]
        public Brand? Brand { get; set; }

        [NotMapped]
        public Category? Category { get; set; }

    }
}
