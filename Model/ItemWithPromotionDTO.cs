namespace Eletronic_Api.Model
{
    public class ItemWithPromotionDTO
    {
        public int ItemID { get; set; }
        public string? ItemName { get; set; }
        public string? BrandName { get; set; }
        public string? CategoryName { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public bool ItemIsActive { get; set; }

        public string? PromotionName { get; set; }
        public decimal? DiscountPercents { get; set; }
        public string? PromotionDescription { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? PromotionIsActive { get; set; }
        public bool? AlertNotification { get; set; }
    }
}
