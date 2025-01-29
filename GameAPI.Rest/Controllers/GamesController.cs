using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameAPI.Models;
using GameAPI.Models.DTO;

namespace GameAPI.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameAPIRestDbContext _context;

        public GamesController(GameAPIRestDbContext context)
        {
            _context = context;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDTO>>> GetGame()
        {
            //return await _context
            //    .Game
            //    .Include(gen => gen.Genre)
            //    .Include(dev => dev.Developer)
            //    .Include(gp => gp.GamePlatforms)
            //        .ThenInclude(p => p.Platform)
            //    .ToListAsync();
            return await _context.Game
                .Include(g => g.Genre)
                .Include(g => g.Developer)
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .Select(g => new GameDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    ReleaseDate = g.ReleaseDate,
                    Genre = new GenreDTO
                    {
                        Id = g.Genre.Id,
                        Name = g.Genre.Name
                    },
                    Developer = new DeveloperDTO
                    {
                        Id = g.Developer.Id,
                        Name = g.Developer.Name,
                        Location = g.Developer.Location
                    },
                    Platforms = g.GamePlatforms.Select(gp => new PlatformDTO
                    {
                        Id = gp.Platform.Id,
                        Name = gp.Platform.Name
                    }).ToList()
                })
                .ToListAsync();
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDTO>> GetGame(int id)
        {
            var game = await _context.Game
                        .Include(g => g.Genre)
                        .Include(g => g.Developer)
                        .Include(g => g.GamePlatforms)
                            .ThenInclude(gp => gp.Platform)
                        .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            var gameDto = new GameDTO
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                ReleaseDate = game.ReleaseDate,
                Genre = new GenreDTO
                {
                    Id = game.Genre.Id,
                    Name = game.Genre.Name
                },
                Developer = new DeveloperDTO
                {
                    Id = game.Developer.Id,
                    Name = game.Developer.Name,
                    Location = game.Developer.Location
                },
                Platforms = game.GamePlatforms.Select(gp => new PlatformDTO
                {
                    Id = gp.Platform.Id,
                    Name = gp.Platform.Name
                }).ToList()
            };

            return gameDto;
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(CreateGameDTO createGameDto)
        {
            //_context.Game.Add(game);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetGame", new { id = game.Id }, game);

            var game = new Game
            {
                Name = createGameDto.Name,
                Description = createGameDto.Description,
                ReleaseDate = createGameDto.ReleaseDate,
                GenreId = createGameDto.GenreId,
                DeveloperId = createGameDto.DeveloperId
            };

            // Create GamePlatform entries for each platform ID
            foreach (var platformId in createGameDto.PlatformIds)
            {
                game.GamePlatforms.Add(new GamePlatform
                {
                    PlatformId = platformId
                });
            }

            _context.Game.Add(game);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Handle foreign key constraint violations
                if (await GameExistsAsync(game.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(
                 nameof(GetGame),
                 new { id = game.Id },
                 await _context.Game
                     .Include(g => g.GamePlatforms)
                     .ThenInclude(gp => gp.Platform)
                     .FirstOrDefaultAsync(g => g.Id == game.Id)
             );
        }

        private async Task<bool> GameExistsAsync(int id)
        {
            return await _context.Game.AnyAsync(e => e.Id == id);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Game.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.Id == id);
        }
    }
}
