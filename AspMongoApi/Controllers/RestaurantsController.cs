using Microsoft.AspNetCore.Mvc;
using AspMongoApi.Models;
using AspMongoApi.Services;

namespace AspMongoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : Controller
    {
        private readonly RestaurantsServices _restaurantsServices;

        public RestaurantsController(RestaurantsServices restaurantsServices) =>
            _restaurantsServices = restaurantsServices;

        [HttpGet]
        public async Task<Restaurant> GetRestaurant(double lat, double lng) =>
            await _restaurantsServices.GetRestaurant(lat, lng);
    }
}