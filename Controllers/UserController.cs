using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Eletronic_Api.Repository.Abastract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eletronic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IUserRepository UserRepository;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;


        public UserController(APIContext context, IConfiguration configuration, IUserRepository UserRepository, IFileService fileService)
        {
            _context = context;
            _configuration = configuration;
            this.UserRepository = UserRepository;
            _fileService = fileService;

        }

        [HttpPost()]
        public IActionResult Login([FromBody] User request)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.UserName == request.UserName && u.Password == request.Password);

            if (user == null)
                return Unauthorized("Invalid username or password");
            user.UserTime = DateTime.Now;
            _context.SaveChanges();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim("IsAdmin", user.IsAdmin.ToString().ToLower())
            }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwt = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = jwt,
                username = user.UserName,
                usertime = user.UserTime.ToString("yyyy-MM-dd HH:mm:ss"),
                profile = user.Profile,
                isAdmin = user.IsAdmin
            });
        }


        [HttpPost("WebUser-register")]
        public async Task<IActionResult> Register([FromForm] User user)
        {

            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest(new { message = "Username and password are required." });

            if (user.ProfileFile != null)
            {
                var fileResult = _fileService.SaveImage(user.ProfileFile);
                if (fileResult.Item1 != 1)
                    return BadRequest(new { message = "Image save failed" });

                user.Profile = fileResult.Item2;
            }
            else
            {

                user.Profile = "/userDefault.png";
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "User for system admin panel Registered successfully." });
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
          var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null)
            {
                return NotFound(user);
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] User User)
        {
            var existingUser = _context.Users.Find(id);
            if (existingUser == null)
                return NotFound("User not found");

            existingUser.UserName = User.UserName;
         
            existingUser.Password = User.Password;
            existingUser.UserTime = DateTime.Now;

            if (User.ProfileFile != null)
            {
                var fileResult = _fileService.SaveImage(User.ProfileFile);
                if (fileResult.Item1 != 1)
                    return BadRequest(new { message = "Image save failed" });

                existingUser.Profile = fileResult.Item2;
            }
            
            _context.Add(User);
            _context.SaveChanges();
          

            return Ok(new { Message = "User updated successfully" });
        }

    
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound("User not found");
            if (!string.IsNullOrEmpty(user.Profile) && user.Profile != "/userDefault.png")
            {
                _fileService.DeleteImage(user.Profile);
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok(new { message = "User deleted successfully" });
        }
    }
}
