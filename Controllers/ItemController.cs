using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Eletronic_Api.Repository.Abastract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eletronic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IItemRepository _itemsReposity;
        private readonly APIContext _dbcontext;


        public ItemController(IFileService fileService, IItemRepository itemsReposity, APIContext context)
        {
            _dbcontext = context;
            _fileService = fileService;
            _itemsReposity = itemsReposity;
        }

        [HttpGet("Category/{categoryId}")]
        public IActionResult GetItemsByCategory(int categoryId)
        {
            var items = _dbcontext.Items
                .Include(i => i.Category)
                .Include(i => i.Brand)
        
                .Where(i => i.CategoryID == categoryId)
                .ToList();

            return Ok(items);
        }

        [HttpGet("Brand/{brandId}")]
        public IActionResult GetItemsByBrand(int brandId)
        {
            var items = _dbcontext.Items
                .Include(i => i.Category)
                .Include(i => i.Brand)
            
                .Where(i => i.BrandID == brandId)
                .ToList();
            return Ok(items);
        }
        [HttpGet("all-item-promotions")]
     

        [HttpGet]
        public IActionResult Index()
        {
            var items = _dbcontext.Items
                 .Include(i => i.Category)
                 .Include(i => i.Brand)

                 .OrderBy(i => i.ItemName)
                 .ToList();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public IActionResult Detail(int id)
        {
            var items = _dbcontext.Items.Include(i => i.Category).Include(i => i.Brand).FirstOrDefault(i => i.ItemID == id);
            if (items == null)
            {
                return NotFound(new { message = "Item not found" });
            }
            return Ok(items);
        }


  
        [HttpPost]
        public  IActionResult Post([FromForm] Item item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (item.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(item.ImageFile);
                if (fileResult.Item1 != 1)
                    return BadRequest(new { message = "Image save failed" });
                item.Image = fileResult.Item2;
            }
            else
            {
                item.Image = "/defualt.png";
            }

            if (item.CategoryID == 0 && !string.IsNullOrEmpty(item.Category?.CategoryName))
            {
                var category = _dbcontext.Categories.FirstOrDefault(c => c.CategoryName == item.Category.CategoryName);
                if (category == null)
                {
                    category = new Category
                    {
                        CategoryName = item.Category.CategoryName,
                        Description = item.Category.Description ?? "No description"
                    };
                    _dbcontext.Categories.Add(category);
                    _dbcontext.SaveChanges();
                }
                item.CategoryID = category.CategoryID;
            }
            if (item.BrandID == 0 && !string.IsNullOrEmpty(item.Brand   ?.BrandName))
            {
                var brand = _dbcontext.Brands.FirstOrDefault(c => c.BrandName == item.Brand.BrandName);
                if (brand == null)
                {
                    brand = new Brand
                    {
                        BrandName = item.Brand.BrandName,
                      
                    };
                    _dbcontext.Brands.Add(brand);
                    _dbcontext.SaveChanges();
                }
                item.BrandID = brand.BrandID;
            }
      
            else if (item.CategoryID == 0)
            {
                return BadRequest(new { message = "Category is required" });
            }
            else if (item.BrandID == 0)
            {
                return BadRequest(new { message = "Brand is required" });
            }
           

            if (_itemsReposity.Add(item))
            {
                return Ok(new { message = "Item added successfully" });
            }
            else
            {
                return BadRequest(new { message = "Failed to add item" });
            }
        }

    
        [HttpPut("{id}")]
  
        public IActionResult Put(int id, [FromForm] Item item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingItem = _dbcontext.Items.FirstOrDefault(i => i.ItemID == id);
            if (existingItem == null)
                return NotFound(new { message = "Item not found" });
            if (item.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(item.ImageFile);
                if (fileResult.Item1 != 1)
                    return BadRequest(new { message = "Image save failed" });
                existingItem.Image = fileResult.Item2;
            }
            if (item.CategoryID == 0 && !string.IsNullOrEmpty(item.Category?.CategoryName))
            {
                var category = _dbcontext.Categories.FirstOrDefault(c => c.CategoryName == item.Category.CategoryName);
                if (category == null)
                {
                    category = new Category
                    {
                        CategoryName = item.Category.CategoryName,
                        Description = item.Category.Description ?? "No description"
                    };
                    _dbcontext.Categories.Add(category);
                    _dbcontext.SaveChanges();
                }
                existingItem.CategoryID = category.CategoryID;
            }
            else
            {
                existingItem.CategoryID = item.CategoryID;
            }
            if (item.BrandID == 0 && !string.IsNullOrEmpty(item.Brand?.BrandName    ))
            {
                var brand = _dbcontext.Brands.FirstOrDefault(c => c.BrandName == item.Brand.BrandName);
                if (brand == null)
                {
                    brand = new Brand
                    {
                        BrandName = item.Brand.BrandName,
                    };
                    _dbcontext.Brands.Add(brand);
                    _dbcontext.SaveChanges();
                }
                existingItem.BrandID = brand.BrandID;
            }
            else
            {
                existingItem.BrandID = item.BrandID;
            }
            
 
            existingItem.ItemName = item.ItemName;
            existingItem.StockQuantity = item.StockQuantity;
            existingItem.Price = item.Price;
            existingItem.Description = item.Description;
            _dbcontext.Items.Update(existingItem);
            _dbcontext.SaveChanges();

            return Ok(new { message = "Item updated successfully" });
        }

  
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _dbcontext.Items.FirstOrDefault(i => i.ItemID == id);
            if (item == null)
            {
                return NotFound(new { message = "Item not found" });
            }
            if (!string.IsNullOrEmpty(item.Image) && !_fileService.DeleteImage(item.Image))
            {
                return BadRequest(new { message = "Failed to delete image" });
            }
            _dbcontext.Items.Remove(item);
            _dbcontext.SaveChanges();
            return Ok(new { message = "Item deleted successfully" });   
        }
    }
}
