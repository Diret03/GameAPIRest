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
        public async Task<ActionResult<GameResponseDTO>> PutGame(int id, UpdateGameDTO gameDto)
        {
            try
            {
                var game = await _context.Game
                    .Include(g => g.GamePlatforms)
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (game == null)
                {
                    return NotFound($"Game with ID {id} does not exist.");
                }

                // Validate Genre if provided
                if (gameDto.GenreId.HasValue)
                {
                    var genre = await _context.Genre.FindAsync(gameDto.GenreId.Value);
                    if (genre == null)
                    {
                        return BadRequest($"Genre with ID {gameDto.GenreId.Value} does not exist.");
                    }
                    game.GenreId = gameDto.GenreId.Value;
                }

                // Validate Developer if provided
                if (gameDto.DeveloperId.HasValue)
                {
                    var developer = await _context.Developer.FindAsync(gameDto.DeveloperId.Value);
                    if (developer == null)
                    {
                        return BadRequest($"Developer with ID {gameDto.DeveloperId.Value} does not exist.");
                    }
                    game.DeveloperId = gameDto.DeveloperId.Value;
                }

                // Update basic properties
                if (gameDto.Name != null)
                    game.Name = gameDto.Name;
                if (gameDto.Description != null)
                    game.Description = gameDto.Description;
                if (gameDto.ReleaseDate.HasValue)
                    game.ReleaseDate = gameDto.ReleaseDate.Value;

                // Update platforms if provided
                if (gameDto.PlatformIds != null)
                {
                    // Validate all platforms exist before making any changes
                    foreach (var platformId in gameDto.PlatformIds)
                    {
                        var platform = await _context.Platform.FindAsync(platformId);
                        if (platform == null)
                        {
                            return BadRequest($"Platform with ID {platformId} does not exist.");
                        }
                    }

                    // Remove existing platforms
                    _context.GamePlatform.RemoveRange(game.GamePlatforms);

                    // Add new platform associations
                    foreach (var platformId in gameDto.PlatformIds)
                    {
                        game.GamePlatforms.Add(new GamePlatform
                        {
                            GameId = id,
                            PlatformId = platformId
                        });
                    }
                }

                await _context.SaveChangesAsync();

                // Get and return the updated game
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
            catch (DbUpdateConcurrencyException)
            {
                if (!await GameExistsAsync(id))
                {
                    return NotFound($"Game with ID {id} was deleted by another request.");
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the game: {ex.Message}");
            }
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(CreateGameDTO createGameDto)
        {
            //_context.Game.Add(game);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetGame", new { id = game.Id }, game);
            try
            {
                // Validate that Genre exists
                var genre = await _context.Genre.FindAsync(createGameDto.GenreId);
                if (genre == null)
                {
                    return BadRequest($"Genre with ID {createGameDto.GenreId} does not exist.");
                }

                // Validate that Developer exists
                var developer = await _context.Developer.FindAsync(createGameDto.DeveloperId);
                if (developer == null)
                {
                    return BadRequest($"Developer with ID {createGameDto.DeveloperId} does not exist.");
                }

                // Validate that all platforms exist
                foreach (var platformId in createGameDto.PlatformIds)
                {
                    var platform = await _context.Platform.FindAsync(platformId);
                    if (platform == null)
                    {
                        return BadRequest($"Platform with ID {platformId} does not exist.");
                    }
                }

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
                await _context.SaveChangesAsync();

                // Return the created game with full details
                var createdGame = await _context.Game
                    .Include(g => g.Genre)
                    .Include(g => g.Developer)
                    .Include(g => g.GamePlatforms)
                        .ThenInclude(gp => gp.Platform)
                    .FirstOrDefaultAsync(g => g.Id == game.Id);

                return CreatedAtAction(
                    nameof(GetGame),
                    new { id = game.Id },
                    new GameResponseDTO
                    {
                        Id = createdGame.Id,
                        Name = createdGame.Name,
                        Description = createdGame.Description,
                        ReleaseDate = createdGame.ReleaseDate,
                        Genre = new GenreDTO
                        {
                            Id = createdGame.Genre.Id,
                            Name = createdGame.Genre.Name
                        },
                        Developer = new DeveloperDTO
                        {
                            Id = createdGame.Developer.Id,
                            Name = createdGame.Developer.Name,
                            Location = createdGame.Developer.Location
                        },
                        Platforms = createdGame.GamePlatforms.Select(gp => new PlatformDTO
                        {
                            Id = gp.Platform.Id,
                            Name = gp.Platform.Name
                        }).ToList()
                    }
                );
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
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
