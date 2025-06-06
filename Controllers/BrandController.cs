using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Eletronic_Api.Repository.Abastract;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eletronic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly APIContext _dbcontext;
        private readonly IFileService _fileService;
        private readonly IBrandRepository _brandRepository;
        public BrandController(IFileService fileService,IBrandRepository brandRepository , APIContext dbcontext)
        {
            _dbcontext = dbcontext;

            _fileService = fileService;
            _brandRepository = brandRepository;
        }

        [HttpGet]
       public IActionResult index()
        {
            var brand=_dbcontext.Brands.ToList();
            return Ok(brand);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id )
        {
            var brand = _dbcontext.Brands.FirstOrDefault(b => b.BrandID == id);
            if (brand == null)
            {
                return NotFound();
            }
            return Ok(brand);
        }


        [HttpPost]
        public IActionResult Post([FromForm] Brand brand)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (brand.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(brand.ImageFile);
                if (fileResult.Item1 != 1)
                    return BadRequest(new { message = "Image save failed" });
                brand.Image = fileResult.Item2;
            }
            else
            {
                brand.Image = "/defaul.jpg";

            }
            _dbcontext.Brands.Add(brand);
            _dbcontext.SaveChanges();
            return Ok(new {message = "Brand added successfully" });  
        }   

     
        [HttpPut("{id}")]
        public IActionResult Put(int id ,[FromForm] Brand brand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingBrand= _dbcontext.Brands.FirstOrDefault(i => i.BrandID == id);
            if (existingBrand == null)
                return NotFound(new { message = "Brand not found" });
            if (brand.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(brand.ImageFile);
                if (fileResult.Item1 != 1)
                    return BadRequest(new { message = "Image save failed" });
                existingBrand.Image = fileResult.Item2;
            }


            existingBrand.BrandName = brand.BrandName;
            _dbcontext.Brands.Update(existingBrand);
            _dbcontext.SaveChanges();

            return Ok(new { message = "Brand updated successfully" });
        }

        // DELETE api/<BrandController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var brand = _dbcontext.Brands.FirstOrDefault(b => b.BrandID == id);
            if (brand == null)
            {
                return NotFound(new { message = "Brand not found" });
            }
            _dbcontext.Brands.Remove(brand);
            _dbcontext.SaveChanges();
            return Ok(new { message = "Brand deleted successfully" });
        }
    }
}
