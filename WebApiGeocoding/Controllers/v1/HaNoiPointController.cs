using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiGeocoding.Models;
using WebApiGeocoding.Repositories;
using WebApiGeocoding.ViewModels;

namespace WebApiGeocoding.Controllers.v1
{
    //[Route("api/v1/[controller]")]
    [Route("api/v1/")]
    [ApiController]
    public class HaNoiPointController : ControllerBase
    {
        private readonly IHaNoiPointReporitory _haNoiPointReporitory;

        public HaNoiPointController(IHaNoiPointReporitory haNoiPointReporitory)
        {
            _haNoiPointReporitory = haNoiPointReporitory;
        }

        [HttpGet]
        [Route("AutoSuggestSearch")]
        public async Task<IEnumerable<HaNoiPointSearch>> AutoSuggestSearch(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) keyword = "";
            return await _haNoiPointReporitory.AutoSuggestSearch(keyword);
        }

        [HttpGet]
        [Route("Geocode")]
        public async Task<GeocodeHaNoiPointSearch> Geocode(double latitude, double longitude)
        {
            return await _haNoiPointReporitory.SearchByLatLong(latitude, longitude);
        }

        [HttpGet]
        [Route("Search")]
        public async Task<GeocodeHaNoiPointSearch> Search(string name)
        {
            return await _haNoiPointReporitory.SearchByName(name);
        }

        [HttpGet]
        [Route("Roadinfo")]
        public async Task<SpeedHaNoiPointSearch> Roadinfo(double latitude, double longitude)
        {
            return await _haNoiPointReporitory.SearchSpeed(latitude, longitude);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(HaNoiPoint haNoiPoint)
        {
            haNoiPoint.SetLocation(haNoiPoint.Longitude, haNoiPoint.Latitude);
            var id = await _haNoiPointReporitory.Create(haNoiPoint);

            return new JsonResult(id.ToString());
        }

        [HttpPut]
        [Route("UpdateSpeed")]
        public async Task<IActionResult> UpdateSpeed(HaNoiPoint haNoiPoint)
        {
            var result = await _haNoiPointReporitory.UpdateSpeed(haNoiPoint.Latitude, haNoiPoint.Longitude, haNoiPoint);

            return new JsonResult(result);
        }

        //[HttpDelete("{id}")]
        //[Route("Delete")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    var result = await _haNoiPointReporitory.Delete(ObjectId.Parse(id));

        //    return new JsonResult(result);
        //}
    }
}
