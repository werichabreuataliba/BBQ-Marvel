using MarvelCharacters.API.Models;
using MarvelCharacters.API.Services.Db;
using MarvelCharacters.API.Services.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelCharacters.API.Controllers
{
    [Route("api/[controller]")]
    public class CharactersController : Controller
    {
        private readonly ILogger<CharactersController> _logger;

        public CharactersController(ILogger<CharactersController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index([FromQuery]string searchString,
            [FromServices] MarvelApi marvelApi,
            [FromServices] MongoDatabase mongoDatabase)
        {
            _logger.LogInformation("Listing characters");

            var apiCharacters = await marvelApi.GetCharacters(searchString);
            var likedCharacters = await mongoDatabase.GetLikes();

            for (int i = 0; i < apiCharacters.Count; i++)
            {
                var currentCharacter = apiCharacters.ToArray()[i];

                var liked = likedCharacters.FirstOrDefault(x => x.Id == currentCharacter.Id);
                if (liked != null)
                {
                    //foi liked
                    currentCharacter.Liked = true;
                }
            }



            return Ok(apiCharacters);
        }


        [HttpPost("{id}/likes")]
        public async Task<IActionResult> CreateLike(
            [FromRoute]int? id,
            [FromBody]Character character,
            [FromServices] MongoDatabase mongoDatabase)
        {
            if (id.HasValue == false || character == null)
                return BadRequest();

            _logger.LogInformation("Create like for id {ID}", id.Value);

            var data = await mongoDatabase.AddLike(character);

            return Ok(data);
        }

        [HttpDelete("{id}/likes")]
        public async Task<IActionResult> DeleteLike(
            [FromRoute]int? id,
            [FromServices] MongoDatabase mongoDatabase)
        {
            if (id.HasValue == false)
                return BadRequest();

            _logger.LogInformation("Delete like for id {ID}", id.Value);

            await mongoDatabase.RemoveLike(id.Value);

            return Ok();
        }
    }
}
