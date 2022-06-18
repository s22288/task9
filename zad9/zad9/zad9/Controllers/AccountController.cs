using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using zad9.Service;

namespace zad9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IDbService _dbService;
        public AccountController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            //var hasher = new PasswordHasher<LoginRequest>();
            //var hashedPassword = hasher.HashPassword(request, request.Password);
            var claims = new Claim[]
                 {
                new (ClaimTypes.Name,"jan123"),
                new("Custom","SomeData"),
                new Claim(ClaimTypes.Role, "admin")
                 };
            var secret = "fjdlkajkfljdakl;fjkda;ljf;ladfpodijafiopjdsoafjidajpfjdoajfiodpjafopjdaopjfdopaijfoipdaopfjdoapjfopdajfopjdaopjfopdajofpjopafjoipdjaopfjdipoajfoipdjaofpjdoajvpodajmop";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var options = new JwtSecurityToken("https://localhost:5001", "https://localhost:5001", claims, expires: DateTime.UtcNow.AddMinutes(5), signingCredentials: creds);
            var refreshToken = "";
            using(var getNum = RandomNumberGenerator.Create())
            {
                var r = new byte[1024];
                getNum.GetBytes(r);
                refreshToken = Convert.ToBase64String(r);
            }
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(options), refreshToken });

        }
        

        [Authorize]
        [HttpGet("secret")]
        public IActionResult LoginUser(User user)
        {
            LoginRequest request = new LoginRequest();
            var hasher = new PasswordHasher<LoginRequest>();
           var hashedPassword = hasher.HashPassword(request, user.Password);

            user.Password = hashedPassword;
            _dbService.saveUser(user);

            // save user to database and return refresh token
            var claims = new Claim[]
                 {
                new (ClaimTypes.Name,user.FristName),
                new("Custom","SomeData"),
                new Claim(ClaimTypes.Role, "admin")
                 };
            var secret = "fjdlkajkfljdakl;fjkda;ljf;ladfpodijafiopjdsoafjidajpfjdoajfiodpjafopjdaopjfdopaijfoipdaopfjdoapjfopdajfopjdaopjfopdajofpjopafjoipdjaopfjdipoajfoipdjaofpjdoajvpodajmop";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var options = new JwtSecurityToken("https://localhost:5001", "https://localhost:5001", claims, expires: DateTime.UtcNow.AddMinutes(5), signingCredentials: creds);
            var refreshToken = "";
            using (var getNum = RandomNumberGenerator.Create())
            {
                var r = new byte[1024];
                getNum.GetBytes(r);
                refreshToken = Convert.ToBase64String(r);
            }
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(options), refreshToken });

        }
    }
}
