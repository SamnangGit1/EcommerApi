using System.ComponentModel.DataAnnotations;

namespace Eletronic_Api.Model
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string? InvoiceNo { get; set; }
        public int AppUserID { get; set; }
        public int StuffID { get; set; }    
        public DateTime PaymenthDate { get; set; }
        public string? PaymentMethod { get; set; }
        public bool IsPay { get; set; }
        public string? DeliveryStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string? Note { get; set; }
        public AppUser? AppUser { get; set; }
        public Staff? Staff { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }





    }
}
