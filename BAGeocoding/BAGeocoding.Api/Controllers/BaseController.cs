using Microsoft.AspNetCore.Mvc;

namespace BAGeocoding.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseController: ControllerBase
    {
    }
}
