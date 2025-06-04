using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Eletronic_Api.Repository.Abastract;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eletronic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly APIContext _dbcontext;
        public CategoryController(APIContext context, IFileService fileService, ICategoryRepository _categoryRepository)
        {
            _dbcontext = context;
            _fileService = fileService;
            this._categoryRepository = _categoryRepository;
        }

        [HttpGet]
        public IActionResult index()
        {
            var category = _dbcontext.Categories.ToList();
            return Ok(category);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _dbcontext.Categories.FirstOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }


        [HttpPost]
        public IActionResult Post([FromForm] Category category)
        {
            if (!ModelState.IsValid)

                return BadRequest(ModelState);
            if (category.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(category.ImageFile);
                if (fileResult.Item1 != 1)
                    return BadRequest(new { message = "Image save failed" });
                category.Image = fileResult.Item2;
            }
            else
            {
                category.Image = "/Images/defaul.jpg";
            }
            _dbcontext.Categories.Add(category);
            var saveResult = _dbcontext.SaveChanges() > 0;
            return Ok(new { message = "Category Added Successfuly", });


        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCategory = _dbcontext.Categories.FirstOrDefault(i => i.CategoryID == id);
            if (existingCategory == null)
                return NotFound(new { message = "Category not found" });
            if (category.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(category.ImageFile);
                if (fileResult.Item1 != 1)
                    return BadRequest(new { message = "Image save failed" });
                existingCategory.Image = fileResult.Item2;
            }


            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Description = category.Description;
            existingCategory.Status = category.Status;

            _dbcontext.Categories.Update(existingCategory);
            _dbcontext.SaveChanges();

            return Ok(new { message = "Category updated successfully" });
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _dbcontext.Categories.Find(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            _dbcontext.Categories.Remove(category);
            _dbcontext.SaveChanges();
            return Ok(new { message = "Category deleted successfully" });

        }
    }
}
