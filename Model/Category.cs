using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eletronic_Api.Model
{
    public class Category
    {
     
        [Key]
        public int CategoryID { get; set; }
        [Required]
        public string? CategoryName { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public bool Status { get; set; } = true;

    }
}
