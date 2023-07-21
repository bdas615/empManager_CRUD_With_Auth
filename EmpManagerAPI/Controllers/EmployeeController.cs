using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using EmpManagerAPI.DataContext;
using EmpManagerAPI.Models.Repositories;
using EmpManagerAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmpManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly EmpDbContextClass dbContext;
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeController(EmpDbContextClass dbContext, 
                                  IEmployeeRepository employeeRepository, 
                                  IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.employeeRepository = employeeRepository;
            this._configuration = configuration;
        }

        // get all details
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<EmployeeParameters>>> GetAllEmployeeDetails()
        {
            var result = await employeeRepository.GetAllEmployees();
            return Ok(result);
        }

        // get Id
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeParameters>> GetEmployee(int id)
        {

            var result = await employeeRepository.GetEmployeeById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // new employee create
        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<ActionResult<EmployeeParameters>> CreateEmployee([FromBody] EmployeeParameters emp)
        {
            try
            {
                if (emp == null)
                {
                    return BadRequest();
                }
                var createdEmployee = await employeeRepository.AddEmployee(emp);
                return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.UserID }, createdEmployee);
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error fetching data from server");
            }
        }

        // update employee
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeParameters>> UpdateEmployee(int id, EmployeeParameters emp)
        {
            try
            {
                var employeeToUpdate = await employeeRepository.GetEmployeeById(id);

                if (employeeToUpdate == null)
                {
                    return NotFound($"Employee with id= {id} not found");
                }

                return await employeeRepository.UpdateEmployee(id, emp);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error fetching data from server");
            }
        }

        [AllowAnonymous]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<EmployeeParameters>> DeleteEmployee(int id)
        {
            try
            {
                var employeeToDelete = await employeeRepository.GetEmployeeById(id);

                if (employeeToDelete == null)
                {
                    return NotFound($"Employee with id = {id} not found");
                }

                return await employeeRepository.DeleteEmployee(id);
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error fetching data from server");
            }
        }
    }
}
//https://localhost:7212/