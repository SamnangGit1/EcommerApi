using Eletronic_Api.Data;
using Eletronic_Api.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Eletronic_Api.Repository.Abastract;



namespace Eletronic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppLoginController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly IAppUserRepository appUserRepository;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;

        public AppLoginController(APIContext context, IConfiguration configuration,IAppUserRepository appUserRepository, IFileService fileService)
        {
            _context = context;
            _configuration = configuration;
            this.appUserRepository = appUserRepository;
            _fileService = fileService;
        }

        [HttpPost()]
        public IActionResult Login([FromBody] AppUser request)
        {
            // Check username & password from database
            var user = _context.AppUsers
                .FirstOrDefault(u => u.UserName == request.UserName && u.Password == request.Password);

            if (user == null)
                return Unauthorized("Invalid username or password");

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName ?? ""),
                    new Claim(ClaimTypes.Email, user.Email ?? "")
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwt = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = jwt,
                username = user.UserName,
                email = user.Email,
                profile = user.Profile
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromForm] AppUser appUser)
        {
            if (appUser.ProfileFile != null)
            {
                var fileResult = _fileService.SaveImage(appUser.ProfileFile);
                if (fileResult.Item1 != 1)
                    return BadRequest(new { message = "Image save failed" });
                appUser.Profile = fileResult.Item2;
            }
            else
            {
                appUser.Profile = "/default.jpg";
            }

            _context.AppUsers.Add(appUser);
            _context.SaveChanges();

            return Ok("User registered successfully");
        }
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromForm] AppUser appUser)
        {
            var existingUser = _context.AppUsers.Find(id);
            if (existingUser == null)
                return NotFound("User not found");

            existingUser.UserName = appUser.UserName;
            existingUser.Email = appUser.Email;
            existingUser.Password = appUser.Password;

            if (appUser.ProfileFile != null)
            {
                var fileResult = _fileService.SaveImage(appUser.ProfileFile);
                if (fileResult.Item1 != 1)
                    return BadRequest(new { message = "Image save failed" });

                existingUser.Profile = fileResult.Item2;
            }

            _context.SaveChanges();

            return Ok("User updated successfully");
        }


    }
}
