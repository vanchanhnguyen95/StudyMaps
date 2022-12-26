using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoExample.Services;
using MongoExample.Models;

namespace MongoExample.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class PlaylistController : Controller
    {
        private readonly MongoDBService _mongoDBService;


        public PlaylistController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<List<Playlist>> Get() {
           return await _mongoDBService.GetAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Playlist playlist) {
            await _mongoDBService.CreateAsync(playlist);
            return CreatedAtAction(nameof(Get), new {id = playlist.Id}, playlist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AddToPlayList(string id, [FromBody] string movieId) {
            await _mongoDBService.AddToPlaytlistAsync(id, movieId);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) {
            await _mongoDBService.DeleteAsync(id);
            return NoContent();
        }
    }
}