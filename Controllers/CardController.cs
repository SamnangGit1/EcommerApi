using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eletronic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        // GET: api/<CardController>
        private readonly APIContext _context;
        public CardController(APIContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var cards = _context.ItemCards
                .Include(i => i.Item)

                .Include(i => i.AppUser)
                .Select(i => new
                {
                    i.CardID,
                    i.Quantity,
                    i.Price,
                    i.Discount,
                    i.IsPromotion,
                    i.Description,
                    i.FinalPrice,
                    i.TotalPrice,
                    ItemName = i.Item.ItemName,

                    Username = i.AppUser.UserName
                })
                .ToList();

            return Ok(cards);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var card = _context.ItemCards.Include(i => i.ItemID).Include(i => i.AppUserID).FirstOrDefault(c => c.CardID == id);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card);
        }

        [HttpGet("GetItemCards")]
        public async Task<ActionResult<IEnumerable<ItemCard>>> GetItemCards([FromQuery] bool? hasPromotion)
        {
            var query = _context.ItemCards
                .Include(ic => ic.Item)

                .Include(ic => ic.AppUser)
                .AsQueryable();



            var itemCards = await query.ToListAsync();

            return itemCards;
        }
        [HttpGet("user/{userId}")]
        public IActionResult GetUserCartItems(int userId)
        {
            try
            {
                var userCartItems = _context.ItemCards
                    .Include(i => i.Item)
                        .ThenInclude(item => item.Brand)
                    .Include(i => i.Item)
                        .ThenInclude(item => item.Category)

                    .Include(i => i.AppUser)
                    .Where(i => i.AppUserID == userId)
                    .Select(i => new
                    {
                        cardID = i.CardID,
                        itemID = i.ItemID,

                        appUserID = i.AppUserID,
                        quantity = i.Quantity,
                        price = i.Price,
                        discount = i.Discount,
                        isPromotion = i.IsPromotion,
                        description = i.Description,
                        finalPrice = i.FinalPrice,
                        totalPrice = i.TotalPrice,
                        itemName = i.Item.ItemName,
                        brandName = i.Item.Brand != null ? i.Item.Brand.BrandName : null,
                        categoryName = i.Item.Category != null ? i.Item.Category.CategoryName : null,
                        item = new
                        {
                            itemID = i.Item.ItemID,
                            itemName = i.Item.ItemName,
                            price = i.Item.Price,
                            description = i.Item.Description,
                            image = i.Item.Image,
                            brand = i.Item.Brand != null ? new
                            {
                                brandID = i.Item.Brand.BrandID,
                                brandName = i.Item.Brand.BrandName
                            } : null,
                            category = i.Item.Category != null ? new
                            {
                                categoryID = i.Item.Category.CategoryID,
                                categoryName = i.Item.Category.CategoryName
                            } : null
                        },

                    })
                    .ToList();

                return Ok(userCartItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody] ItemCard itemCard)
        {
            try
            {
                _context.Add(itemCard);
                _context.SaveChanges();
                return Ok(new { message = "Card Add Successful" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { error = ex.InnerException?.Message ?? ex.Message });
            }
        }



        [HttpPut("{id}")]
        public IActionResult Put([FromBody] ItemCard itemCard, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingItem = _context.ItemCards.FirstOrDefault(i => i.CardID == id);
            if (existingItem == null)
                return NotFound(new { message = "Item not found" });

            if (itemCard.ItemID == 0 && !string.IsNullOrEmpty(itemCard.Item?.ItemName))
            {
                var item = _context.Items.FirstOrDefault(c => c.ItemName == itemCard.Item.ItemName);
                if (item == null)
                {
                    item = new Item
                    {
                        ItemName = itemCard.Item.ItemName,
                        Description = itemCard.Item.Description ?? "No description",
                        Price = itemCard.Item.Price,
                        Image = itemCard.Item.Image ?? "/default.jpg"
                    };

                    _context.Items.Add(item);
                    _context.SaveChanges();
                }
                existingItem.ItemID = item.ItemID;
            }
            else
            {
                existingItem.ItemID = itemCard.ItemID;
            }
            if (itemCard.AppUserID == 0 && !string.IsNullOrEmpty(itemCard.AppUser?.UserName))
            {
                var appuser = _context.AppUsers.FirstOrDefault(c => c.UserName == itemCard.AppUser.UserName);
                if (appuser == null)
                {
                    appuser = new AppUser
                    {
                        UserName = itemCard.AppUser.UserName,
                        Email = itemCard.AppUser.Email ?? "No email",
                        Phone = itemCard.AppUser.Phone ?? "No phone number",
                        Address = itemCard.AppUser.Address ?? "No address",
                        AddressType = itemCard.AppUser.AddressType ?? "No address type",
                        HouseNo = itemCard.AppUser.HouseNo ?? "No house number",
                        IsVerified = itemCard.AppUser.IsVerified

                    };


                    _context.AppUsers.Add(appuser);
                    _context.SaveChanges();
                }
                existingItem.AppUserID = appuser.AppUserID;
            }
            else
            {
                existingItem.AppUserID = itemCard.AppUserID;
            }


            existingItem.Quantity = itemCard.Quantity;
            existingItem.Price = itemCard.Price;
            existingItem.Discount = itemCard.Discount;
            existingItem.IsPromotion = itemCard.IsPromotion;
            existingItem.Description = itemCard.Description;
            _context.ItemCards.Update(existingItem);
            _context.SaveChanges();
            return Ok(new { message = "Card updated successfully" });

        }
        // DELETE api/<CardController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var itemCard = _context.ItemCards.FirstOrDefault(i => i.CardID == id);
            if (itemCard == null)
            {
                return NotFound(new { message = "Card not found" });
            }
            _context.ItemCards.Remove(itemCard);
            _context.SaveChanges();
            return Ok(new { message = "Card deleted successfully" });
        }
    }
}
