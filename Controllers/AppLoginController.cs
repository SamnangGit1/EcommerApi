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

        private readonly IEmailService _emailService;
        public AppLoginController(APIContext context, IConfiguration configuration,IAppUserRepository appUserRepository, IFileService fileService, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            this.appUserRepository = appUserRepository;
            _fileService = fileService;
            _emailService = emailService;
        
        }

        [HttpPost()]
        public IActionResult Login([FromBody] AppUser request)
        {
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
                   new Claim(ClaimTypes.Email, user.Email ?? ""),
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
        public async Task<IActionResult> Register([FromForm] AppUser appUser)
        {
            if (string.IsNullOrWhiteSpace(appUser.Email))
                return BadRequest(new { message = "Email is required." });

            if (string.IsNullOrWhiteSpace(appUser.UserName) || string.IsNullOrWhiteSpace(appUser.Password))
                return BadRequest(new { message = "Username and password are required." });

            // Save image if provided
            if (appUser.ProfileFile != null)
            {
                var fileResult = _fileService.SaveImage(appUser.ProfileFile);
                if (fileResult.Item1 != 1)
                    return BadRequest(new { message = "Image save failed" });

                appUser.Profile = fileResult.Item2; 
            }
            else
            {
              
                appUser.Profile = "/userDefault.png";
            }

            appUser.IsVerified = false;
        

            _context.AppUsers.Add(appUser);
            await _context.SaveChangesAsync();

            var otp = new Random().Next(100000, 999999).ToString();

            _context.OtpStores.Add(new OtpStore
            {
                Email = appUser.Email,
                Otp = otp,
                ExpiryTime = DateTime.Now.AddMinutes(5)
            });
            await _context.SaveChangesAsync();

            _emailService.SendOtp(appUser.Email, otp);

            return Ok(new { message = "Registered successfully. OTP sent to email." });
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

            return Ok(new { Message = "User updated successfully" });
        }

        [HttpPost("send-otp")]
        public IActionResult SendOtp([FromBody] AppUser request)
        {
            var user = _context.AppUsers.FirstOrDefault(u => u.Email == request.Email);
            if (user == null) return NotFound("Email not registered");

            var otp = new Random().Next(100000, 999999).ToString();

            _context.OtpStores.Add(new OtpStore
            {
                Email = request.Email,
                Otp = otp,
                ExpiryTime = DateTime.Now.AddMinutes(5)
            });
            _context.SaveChanges();

            _emailService.SendOtp(request.Email, otp);
            return Ok("OTP sent to email");
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] OtpVerifyRequest request)
        {
            var otpEntry = _context.OtpStores
                .Where(x => x.Email == request.Email && x.Otp == request.Otp)
                .OrderByDescending(x => x.ExpiryTime)
                .FirstOrDefault();

            if (otpEntry == null || otpEntry.ExpiryTime < DateTime.Now)
                return BadRequest("Invalid or expired OTP");

            var user = _context.AppUsers.FirstOrDefault(u => u.Email == request.Email);
            if (user == null) return NotFound("User not found");

   
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
            var jwt = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = jwt,
                username = user.UserName,
                email = user.Email,
                profile = user.Profile
            });
        }



    }

}
