﻿using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



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


    }
}
