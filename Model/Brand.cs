using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eletronic_Api.Model
{
    public class Brand
    {
         [Key]
          public int BrandID { get; set; }
         [Required]
         public string? BrandName { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public ICollection<Item>? Items { get; set; }

    }
}
