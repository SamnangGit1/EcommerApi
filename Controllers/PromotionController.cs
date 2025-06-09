using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eletronic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly APIContext _dbcontext;
        public PromotionController(APIContext context)
        {
            _dbcontext = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var promotions = _dbcontext.Promotions.ToList();
            if (promotions == null || !promotions.Any())
            {
                return NotFound("No promotions found.");
            }
            return Ok(promotions);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var promotion = _dbcontext.Promotions.FirstOrDefault(p => p.PromotionID == id);
            if (promotion == null)
            {
                return NotFound($"Promotion with ID {id} not found.");
            }
            return Ok(promotion);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Promotion promotion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbcontext.Add(promotion);
            _dbcontext.SaveChanges();
            return Ok(new { message = "Promotion Added Successfully" });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Promotion promotion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPromotion = _dbcontext.Promotions.FirstOrDefault(p => p.PromotionID == id);
            if (existingPromotion == null)
            {
                return NotFound($"Promotion with ID {id} not found.");
            }

            existingPromotion.PromotionName = promotion.PromotionName;
            existingPromotion.DiscountType = promotion.DiscountType;
            existingPromotion.DiscountValue = promotion.DiscountValue;
            existingPromotion.Description = promotion.Description;
            existingPromotion.StartDate = promotion.StartDate;
            existingPromotion.EndDate = promotion.EndDate;
            existingPromotion.IsActive = promotion.IsActive;

            _dbcontext.SaveChanges();
            return Ok(new { message = "Promotion Updated Successfully" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var promotion = _dbcontext.Promotions.FirstOrDefault(p => p.PromotionID == id);
            if (promotion == null)
            {
                return NotFound($"Promotion with ID {id} not found.");
            }

            _dbcontext.Promotions.Remove(promotion);
            _dbcontext.SaveChanges();
            return Ok(new { message = "Promotion Deleted Successfully" });
        }
    }
}
