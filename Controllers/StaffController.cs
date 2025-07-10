using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Eletronic_Api.Repository.Abastract;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eletronic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IFileService _fileService;
        private readonly IStaffRepository _staffRepository;
        public StaffController(APIContext context,IStaffRepository staffrepo,IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
            _staffRepository = staffrepo;

        }
        [HttpGet]
        public IActionResult index()
        {
            var staff= _context.staffs.ToList();
            if (staff == null || !staff.Any())
            {
                return NotFound("No staff found.");
            }
            return Ok(staff);
        }
        // GET api/<StaffController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
           var staff =_context.staffs.FirstOrDefault(s=>s.StaffID == id);
            if (staff == null)
            {
                return NotFound(
                    $"Staff with ID {id} not found.");
            }
            return Ok(staff);
        }

        // POST api/<StaffController>
        [HttpPost]
        public IActionResult Post([FromForm] Staff staff)
        {
            if (staff == null)
            {
                return BadRequest("Staff data is null.");
            }
            if (staff.ProfileFile != null)
            {
                var fileResult = _fileService.SaveImage(staff.ProfileFile);
                if (fileResult.Item1 != 1)
                    return BadRequest(new { message = "Image save failed" });
                staff.Profile = fileResult.Item2;
            }
            else
            {
                staff.Profile = "/userDefault.png";
            }
            _context.staffs.Add(staff);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = staff.StaffID }, staff);
        }

        // PUT api/<StaffController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Staff staff)
        {
            var exitingstaff = _context.staffs.FirstOrDefault(s => s.StaffID == id);
            if (exitingstaff == null)
            {
                return NotFound($"Staff with ID {id} not found.");
            }
            if (exitingstaff.ProfileFile != null)
            {
                var fileResult = _fileService.SaveImage(exitingstaff.ProfileFile);
                if (fileResult.Item1 != 1)
                    return BadRequest(new { message = "Image save failed" });
                exitingstaff.Profile = fileResult.Item2;

            }
            
            exitingstaff.StaffName = staff.StaffName;
            exitingstaff.Sex = staff.Sex;
            exitingstaff.Email = staff.Email;
            exitingstaff.Phone = staff.Phone;
            exitingstaff.Address = staff.Address;
            exitingstaff.Department = staff.Department;
            exitingstaff.Salary = staff.Salary;
            exitingstaff.HiredDate = staff.HiredDate;
            _context.Update(exitingstaff);
            _context.SaveChanges();
            return Ok(new { message = "Staff updated successfully" });
        }

        // DELETE api/<StaffController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var staff = _context.staffs.FirstOrDefault(s=>s.StaffID == id);
            if (staff == null) return BadRequest();
            _context.staffs.Remove(staff);  
            _context.SaveChanges();
            return Ok(staff);   
        }

    }
}
