using Elastic02.Models;
using Elastic02.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elastic02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IElasticService<Employee> _employeeService;
        public EmployeeController(IElasticService<Employee> employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        [Route("addemployee")]
        public async Task<IActionResult> AddDocumentAsync([FromForm] Employee employee)
        {
            await _employeeService.AddDocumentAsync(employee);
            return Ok();
        }


        [HttpPost]
        [Route("updateemployee")]
        public async Task<IActionResult> UpdateEmployee([FromForm] Employee employee)
        {
            await _employeeService.UpdateDocumentAsync(employee);
            return Ok();
        }


    }
}
