using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eletronic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPermissionController : ControllerBase
    {
        private readonly APIContext _context;
        public UserPermissionController(APIContext context)
        {
            _context = context;
        }
        // GET: api/<UserPermissionController>
        [HttpGet]
       public IActionResult index()
        {
            var permissions = _context.UserPermissions.ToList();
            if (permissions == null || !permissions.Any())
            {
                return NotFound("No permissions found.");
            }
            return Ok(permissions);
        }

        // GET api/<UserPermissionController>/5
        [HttpGet("{id}")]
       public IActionResult Get(int id)
        {
            var permission = _context.UserPermissions.FirstOrDefault(p => p.UserPermissionID == id);
            if (permission == null)
            {
                return NotFound("Permission not found.");
            }
            return Ok(permission);
        }

        // POST api/<UserPermissionController>
        [HttpPost]
        public IActionResult Post([FromBody] UserPermissionRequest request)
        {
            var permissions = request.PermissionName.Select(permissionName => new Userpermission
            {
                UserID = request.UserID,
                PermissionName = permissionName
            }).ToList();

            _context.UserPermissions.AddRange(permissions);
            _context.SaveChanges();
            return Ok(new { message = "Permissions added successfully!" });
        }


        // PUT api/<UserPermissionController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Userpermission userpermission)
        {
            var exitingUserpermission = _context.UserPermissions.FirstOrDefault(p => p.UserPermissionID == id);
            if (exitingUserpermission == null)
            {
                return NotFound("Permission not found.");
            }
           
        
            _context.UserPermissions.Update(exitingUserpermission);
            _context.SaveChanges();
            return Ok(new { message = "Permission updated successfully!" });
        }

        // DELETE api/<UserPermissionController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var permission = _context.UserPermissions.FirstOrDefault(p => p.UserPermissionID == id);
            if (permission == null)
            {
                return NotFound("Permission not found.");
            }
            _context.UserPermissions.Remove(permission);
            _context.SaveChanges();
            return Ok(new { message = "Permission deleted successfully!" });
        }
        [HttpGet("by-user/{userId}")]
        public IActionResult GetPermissionsByUserId(int userId)
        {
            var permissions = _context.UserPermissions
                .Where(p => p.UserID == userId)
                .ToList();

            return Ok(permissions);
        }

    }
}
