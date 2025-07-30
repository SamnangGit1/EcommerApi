using System.ComponentModel.DataAnnotations;

namespace Eletronic_Api.Model
{
    public class ItemCard
    {
        [Key]
        public int CardID { get; set; }
        public int ItemID { get; set; }

        public int AppUserID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool IsPromotion { get; set; }
        public string? Description { get; set; }

        public AppUser? AppUser { get; set; }
        public Item? Item { get; set; }



        public decimal FinalPrice => Price - Discount;
        public decimal TotalPrice => FinalPrice * Quantity;
    }
}
