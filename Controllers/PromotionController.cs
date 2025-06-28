using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("active-promotions")]
        public IActionResult GetActivePromotions()
        {
            var currentDate = DateTime.Now;

            var promotions = _dbcontext.ItemDetails
                .Include(d => d.Promotion)
                .Include(d => d.Item)
                .Where(d =>
                    d.IsActive &&
                    d.Promotion != null &&
                    d.Item != null &&
                    d.StartDate <= currentDate &&
                    d.EndDate >= currentDate
                )
                .Select(d => new
                {
                    Title = d.Promotion.PromotionName,
                    Message = d.Promotion.Description,
                    Discount = d.DiscountPercent,
                    ItemName = d.Item.ItemName,
                    StartDate = d.StartDate.ToString("dd/MM/yyyy"),
                    EndDate = d.EndDate.ToString("dd/MM/yyyy"),
                    DurationInDays = EF.Functions.DateDiffDay(d.StartDate, d.EndDate),
                    DurationInMonths = EF.Functions.DateDiffMonth(d.StartDate, d.EndDate),

                    // 📆 Check if today is within the range
                    IsActiveNow = DateTime.Now >= d.StartDate && DateTime.Now <= d.EndDate,
                    ViewCount = 1 // placeholder if you don't track yet
                })
                .ToList();

            return Ok(promotions);
        }


        [HttpPost]
        public IActionResult Post([FromForm] Promotion promotion)
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
        public IActionResult Put(int id, [FromForm] Promotion promotion)
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
