using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webgis02.Repositories;
using webgis02.Models;

namespace webgis02.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HaLongRoadController : ControllerBase
    {
        private readonly IHaLongRoadRepository _haLongRoadRepository;

        public HaLongRoadController(IHaLongRoadRepository haLongRoadRepository)
        {
            _haLongRoadRepository = haLongRoadRepository;
        }

        [HttpGet("/haLongRoad/getall")]
        public async Task<ActionResult<IEnumerable<HaLongRoad>>> GetAll()
        {
            var wcData = await _haLongRoadRepository.GetAll();
            return Ok(wcData);
        }

        [HttpGet("/haLongRoad/topten")]
        public async Task<ActionResult<IEnumerable<HaLongRoad>>> GetTopTenRoads()
        {
            var wcData = await _haLongRoadRepository.GetTopTenRoads();
            return Ok(wcData);
        }

    }
}