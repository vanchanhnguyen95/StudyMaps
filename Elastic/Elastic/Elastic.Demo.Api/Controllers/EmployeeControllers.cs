using Elastic.Demo.Api.Interface;
using Elastic.Demo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Elastic.Demo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeControllers : Controller
    {
        private readonly ElasticSearchFactory _esFactory;
        
        public EmployeeControllers()
        {
            _esFactory = new ElasticSearchFactory();
        }

        [Route("[action]")]
        [HttpPost, ActionName("updateemployee")]
        public async Task<IActionResult> UpdateEmployee([FromForm] Employee employee)
        {
            
            return Ok();
        }
    }
}
