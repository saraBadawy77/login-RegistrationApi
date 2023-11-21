using loginApi.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
namespace loginApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIRegistrationController : ControllerBase
    {
        APIContext db = new APIContext();








        [HttpPost("Register")]
        public ActionResult<string> Register(registration registrationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the username or email already exists in the database
            if (db.registrations.Any(r => r.username == registrationModel.username || r.Email == registrationModel.Email))
            {
                return BadRequest("Username or email already exists.");
            }

            // Create a new user record and save it in the database
            var user = new registration
            {
                username = registrationModel.username,
                Email = registrationModel.Email,
                password = registrationModel.password
            };

            db.registrations.Add(user);
            db.SaveChanges();

            return Ok("Registration successful");
        }

        // POST: api/Registration/Login
        [HttpPost("Login")]
        public ActionResult<string> Login(Login loginModel)
        {
            // Check the username and password and verify user's identity
            var user = db.registrations.FirstOrDefault(u => u.username == loginModel.Username && u.password == loginModel.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // Create and return a JWT token for the user
            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }
        private string GenerateJwtToken(registration user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("YourSecretKeyforAPPCryptoProviderFactoryCreateKeyedHashAlgorithm"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.username)
            };

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



       





}
}
