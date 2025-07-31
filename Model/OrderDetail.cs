namespace Eletronic_Api.Model
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
       
        public decimal Discount { get; set; }
        public bool IsPromotion { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public Order? Order { get; set; }
        public Item? Item { get; set; }

        public decimal FinalPrice => Price - Discount;
        public decimal TotalPrice => FinalPrice * Quantity;
    }
}
