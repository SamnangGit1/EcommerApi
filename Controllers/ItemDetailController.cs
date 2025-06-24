using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Eletronic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemDetailController : ControllerBase
    {
        private readonly APIContext _context;
        public ItemDetailController(APIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult index()
        {
            var itemdetail=_context.ItemDetails.Include(i => i.Item).Include(p=>p.Promotion).ToList();
            if (itemdetail == null || !itemdetail.Any())
            {
                return NotFound(new { message = "No item details found" });
            }
            return Ok(itemdetail);
        }

   
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
           var itemdetail =_context.ItemDetails.Include(i => i.Item)
                .Include(p => p.Promotion)
                .FirstOrDefault(i => i.ItemDetailID == id);
            if (itemdetail == null)
            {
                return NotFound(new { message = "Item detail not found" });
            }
            return Ok(itemdetail);  
        }

     
        [HttpPost]
        public IActionResult Post([FromForm] ItemDetail itemDetail)
        {

      
            _context.ItemDetails.Add(itemDetail);
            _context.SaveChanges();
            return Ok(new { message = "Item detail added successfully" });
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ItemDetail itemDetail)
        {
            var existingItemDetail = _context.ItemDetails.FirstOrDefault(i => i.ItemDetailID == id);
            if (existingItemDetail == null)
            {
                return NotFound(new { message = "Item detail not found" });
            }
            existingItemDetail.ItemID = itemDetail.ItemID;
            existingItemDetail.PromotionID = itemDetail.PromotionID;
            existingItemDetail.DiscountPercent = itemDetail.DiscountPercent;
            existingItemDetail.Price = itemDetail.Price;
            existingItemDetail.StartDate = itemDetail.StartDate;
            existingItemDetail.EndDate = itemDetail.EndDate;
            existingItemDetail.IsActive = itemDetail.IsActive;
            _context.SaveChanges();
            return Ok(new { message = "Item detail updated successfully" });
        }

   
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var itemDetail = _context.ItemDetails.FirstOrDefault(i => i.ItemDetailID == id);
            if (itemDetail == null)
            {
                return NotFound(new { message = "Item detail not found" });
            }
            _context.ItemDetails.Remove(itemDetail);
            _context.SaveChanges();
            return Ok(new { message = "Item detail deleted successfully" });
        }
    }
}
