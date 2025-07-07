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
        public IActionResult GetAll()
        {
            var promotions = _dbcontext.Promotions.ToList();
            return Ok(promotions);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var promotion = _dbcontext.Promotions.FirstOrDefault(p => p.PromotionID == id);
            if (promotion == null)
                return NotFound("Promotion not found");

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
        public IActionResult Put(int id, [FromForm] Promotion promotion)
        {
            var existing = _dbcontext.Promotions.FirstOrDefault(p => p.PromotionID == id);
            if (existing == null)
                return NotFound("Promotion not found");

            existing.PromotionName = promotion.PromotionName;
            existing.PromotionType = promotion.PromotionType;
            existing.TargetID = promotion.TargetID;
            existing.DiscountPercents = promotion.DiscountPercents;
            existing.Description = promotion.Description;
            existing.StartDate = promotion.StartDate;
            existing.EndDate = promotion.EndDate;
            existing.IsActive = promotion.IsActive;
            existing.AlertNotification = promotion.AlertNotification;

            _dbcontext.SaveChanges();
            return Ok(new { message = "Promotion updated successfully" });
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

        [HttpGet("by-target")]
        public IActionResult GetByTarget([FromQuery] string type, [FromQuery] int id)
        {
            var promotions = _dbcontext.Promotions
                .Where(p => p.PromotionType.ToLower() == type.ToLower() && p.TargetID == id)
                .ToList();

            if (!promotions.Any())
                return NotFound("No promotions found for given target");

            return Ok(promotions);
        }
        [HttpGet("items-with-promotions")]
      
        public async Task<ActionResult<IEnumerable<ItemWithPromotionDTO>>> GetItemsWithPromotions()
        {
            var items = await _dbcontext.Items
                .Include(i => i.Brand)
                .Include(i => i.Category)
                .ToListAsync();

            var promotions = await _dbcontext.Promotions
                .Where(p => p.IsActive)
                .ToListAsync();

            var result = items.Select(i =>
            {
                var itemPromo = promotions.FirstOrDefault(p => p.PromotionType == "Item" && p.TargetID == i.ItemID);
                var categoryPromo = promotions.FirstOrDefault(p => p.PromotionType == "Category" && p.TargetID == i.CategoryID);
              
                var brandPromo = promotions.FirstOrDefault(p => p.PromotionType == "Brand" && p.TargetID == i.BrandID);
                var promo = itemPromo ?? categoryPromo ?? brandPromo;
                if (promo == null)
                {
                    promo = promotions.FirstOrDefault(p => p.PromotionType == "All");
                }

                return new ItemWithPromotionDTO
                {
                    ItemID = i.ItemID,
                    ItemName = i.ItemName,
                    BrandName = i.Brand?.BrandName,
                    CategoryName = i.Category?.CategoryName,
                    StockQuantity = i.StockQuantity,
                    Price = i.Price,
                    ItemIsActive = i.IsActive,
                    PromotionType = promo?.PromotionType,
                    PromotionName = promo?.PromotionName,             
                    DiscountPercents = promo?.DiscountPercents,
                    PromotionDescription = promo?.Description,
                    StartDate = promo?.StartDate,
                    EndDate = promo?.EndDate,
                    PromotionIsActive = promo?.IsActive,
                    AlertNotification = promo?.AlertNotification
                };
            }).ToList();

            return Ok(result);
        }
        [HttpGet("list-promotions")]
        public async Task<IActionResult> GetOnlyItemsWithPromotions()
        {
            var items = await _dbcontext.Items
                .Include(i => i.Brand)
                .Include(i => i.Category)
                .ToListAsync();

            var promotions = await _dbcontext.Promotions
                .Where(p => p.IsActive)
                .ToListAsync();

            var result = new List<ItemWithPromotionDTO>();

            foreach (var item in items)
            {
                // ជ្រើស promotion ដែលត្រូវគ្នា
                var applicablePromotions = promotions.Where(p =>
                    (p.PromotionType == "Item" && p.TargetID == item.ItemID) ||
                    (p.PromotionType == "Brand" && p.TargetID == item.BrandID) ||
                    (p.PromotionType == "Category" && p.TargetID == item.CategoryID) ||
                    (p.PromotionType == "All")
                ).ToList();

                // ប្រសិនបើមាន promotion ត្រឹមតែ១ ឬច្រើន អ្នកអាចជ្រើសមួយឬបញ្ចូលច្រើន
                foreach (var promotion in applicablePromotions)
                {
                    result.Add(new ItemWithPromotionDTO
                    {
                        ItemID = item.ItemID,
                        ItemName = item.ItemName,
                        BrandName = item.Brand?.BrandName,
                        CategoryName = item.Category?.CategoryName,
                        StockQuantity = item.StockQuantity,
                        Price = item.Price,
                        ItemIsActive = item.IsActive,

                        PromotionName = promotion.PromotionName,
                        PromotionType = promotion.PromotionType,
                        DiscountPercents = promotion.DiscountPercents,
                        PromotionDescription = promotion.Description,
                        StartDate = promotion.StartDate,
                        EndDate = promotion.EndDate,
                        PromotionIsActive = promotion.IsActive,
                        AlertNotification = promotion.AlertNotification
                    });
                }
            }

            return Ok(result);
        }

    }


}

