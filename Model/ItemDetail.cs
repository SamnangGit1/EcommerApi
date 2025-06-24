namespace Eletronic_Api.Model
{
    public class ItemDetail
    {
        public int ItemDetailID { get; set; }
        public int ItemID { get; set; }                 
        public int PromotionID { get; set; }
        public double DiscountPercent { get; set; }     
        public double Price { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public Item? Item { get; set; }
        public Promotion? Promotion { get; set; }

    }
}
