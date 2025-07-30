using System.ComponentModel.DataAnnotations.Schema;

namespace Eletronic_Api.Model
{
    public class Item
    {
        public int ItemID { get; set; }
        public int BrandID { get; set; }
        public int CategoryID { get; set; }
        public string? ItemName { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }

        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; }

        public Brand? Brand { get; set; }
        public Category? Category { get; set; }
        public ICollection<ItemCard>? ItemCards { get; set; }


    }
}
