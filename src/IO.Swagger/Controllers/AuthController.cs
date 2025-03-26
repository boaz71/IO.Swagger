using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IO.Swagger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        // קונסטרקטור שמקבל את IConfiguration מה-Dependency Injection
        public AuthController(IConfiguration config)
        {
            _config = config;
        }


        /// <summary>
        /// מחזיר טוקן JWT לבדיקה, ללא צורך באימות משתמש אמיתי.
        /// </summary>
        /// <returns>JWT בתוקף לפי פרמטר שמגיע מההגדרות דקות</returns>
        [HttpGet("token")]
        [AllowAnonymous]
        public IActionResult GetToken()
        {

            var minutes = 30;

            // קריאת המפתח מתוך appsettings.json
            var key = Encoding.ASCII.GetBytes(_config["JwtSettings:SecretKey"]);
            int.TryParse(_config["JwtSettings:TokenExpirationMinutes"], out minutes);

            // יוצר handler עבור יצירת JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            // יצירת פרטי הטוקן
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, _config["JwtSettings:UserName"]) // שם משתמש מההגדרות
                }),
                Expires = DateTime.UtcNow.AddMinutes(minutes), // תוקף הטוקן

                // יצירת חתימה עבור הטוקן
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // יצירת הטוקן
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // יצירת מחרוזת מהטוקן
            var tokenString = tokenHandler.WriteToken(token);

            // מחזיר את הטוקן בפורמט JSON
            return Ok(new { token = tokenString });
        }
    }
}
