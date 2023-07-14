using EmpManagerAPI.DataContext;
using EmpManagerAPI.JwtService;
using EmpManagerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly EmpDbContextClass dbContext;

        public LoginController(IConfiguration configuration, EmpDbContextClass empDbContextClass)
        {
            this._configuration = configuration;
            this.dbContext = empDbContextClass;
        }

        [AllowAnonymous]
        [HttpPost("Create")]
        public IActionResult CreateEmployee(Register emp)
        {
            if (dbContext.RegistrationData.Where(x => x.Email == emp.Email).FirstOrDefaultAsync() != null)
            {
                return Ok("Already Exists");
            }

            emp.CreatedOn = DateTime.Now;
            dbContext.RegistrationData.Add(emp);
            dbContext.SaveChangesAsync();
            return Ok("Success");
        }

        [AllowAnonymous]
        [HttpPost("LoginEmployee")]
        public IActionResult LoginEmployee(Login login)
        {
            var employeeAvailable = dbContext.RegistrationData.Where(x => x.Email == login.Email && x.Pwd == login.Password).FirstOrDefault();
            if (employeeAvailable != null)
            {
                return Ok(new JwtServiceClass(_configuration).GenerateToken(

                    employeeAvailable.UserId.ToString(),
                    employeeAvailable.Name,
                    employeeAvailable.Email
                    ));
            }
            return Ok("Failure");
        }
    }
}


//https://localhost:7212/