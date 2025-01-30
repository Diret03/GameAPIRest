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
        public async Task<ActionResult<IEnumerable<GameResponseDTO>>> GetGame()
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
                .Select(g => new GameResponseDTO
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
        public async Task<ActionResult<GameResponseDTO>> GetGame(int id)
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

            var gameDto = new GameResponseDTO
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
        public async Task<IActionResult> PutGame(int id, UpdateGameDTO gameDto)
        {
            var game = await _context.Game
                   .Include(g => g.GamePlatforms)
                   .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            // Only update properties that are not null in the DTO
            if (gameDto.Name != null)
                game.Name = gameDto.Name;

            if (gameDto.Description != null)
                game.Description = gameDto.Description;

            if (gameDto.ReleaseDate.HasValue)
                game.ReleaseDate = gameDto.ReleaseDate.Value;

            if (gameDto.GenreId.HasValue)
                game.GenreId = gameDto.GenreId.Value;

            if (gameDto.DeveloperId.HasValue)
                game.DeveloperId = gameDto.DeveloperId.Value;

            // Only update platforms if PlatformIds is provided
            if (gameDto.PlatformIds != null)
            {
                // Remove existing platforms
                _context.GamePlatform.RemoveRange(game.GamePlatforms);

                // Add new platform associations
                foreach (var platformId in gameDto.PlatformIds)
                {
                    game.GamePlatforms.Add(new GamePlatform
                    {
                        PlatformId = platformId
                    });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GameExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException)
            {
                return BadRequest("Invalid Genre, Developer, or Platform IDs");
            }

            // Return the updated game
            var updatedGame = await _context.Game
                .Include(g => g.Genre)
                .Include(g => g.Developer)
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .FirstOrDefaultAsync(g => g.Id == id);

            return Ok(new GameResponseDTO
            {
                Id = updatedGame.Id,
                Name = updatedGame.Name,
                Description = updatedGame.Description,
                ReleaseDate = updatedGame.ReleaseDate,
                Genre = new GenreDTO
                {
                    Id = updatedGame.Genre.Id,
                    Name = updatedGame.Genre.Name
                },
                Developer = new DeveloperDTO
                {
                    Id = updatedGame.Developer.Id,
                    Name = updatedGame.Developer.Name,
                    Location = updatedGame.Developer.Location
                },
                Platforms = updatedGame.GamePlatforms.Select(gp => new PlatformDTO
                {
                    Id = gp.Platform.Id,
                    Name = gp.Platform.Name
                }).ToList()
            });
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
