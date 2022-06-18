using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zad9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok("data students");
        }
        [Authorize]
        [HttpGet("secret")]
        public IActionResult GetSecretData()
        {
            
            return Ok("Secrete data...");
        }

        [Authorize(Roles ="admin")]
        [HttpGet("secret-for-admin")]
        public IActionResult GetSecretDataForAdmin()
        {
            return Ok("Secret data for amin...");
        }
    }
}
