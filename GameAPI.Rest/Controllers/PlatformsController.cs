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
    public class PlatformsController : ControllerBase
    {
        private readonly GameAPIRestDbContext _context;

        public PlatformsController(GameAPIRestDbContext context)
        {
            _context = context;
        }

        // GET: api/Platforms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformResponseDTO>>> GetPlatform()
        {
            return await _context.Platform
                .Include(p => p.GamePlatforms)
                    .ThenInclude(gp => gp.Game)
                        .ThenInclude(g => g.Genre)
                .Include(p => p.GamePlatforms)
                    .ThenInclude(gp => gp.Game)
                        .ThenInclude(g => g.Developer)
                .Select(platform => new PlatformResponseDTO
                {
                    Id = platform.Id,
                    Name = platform.Name,
                    Games = platform.GamePlatforms.Select(gp => new GameResponseDTO
                    {
                        Id = gp.Game.Id,
                        Name = gp.Game.Name,
                        Description = gp.Game.Description,
                        ReleaseDate = gp.Game.ReleaseDate,
                        Genre = new GenreDTO
                        {
                            Id = gp.Game.Genre.Id,
                            Name = gp.Game.Genre.Name
                        },
                        Developer = new DeveloperDTO
                        {
                            Id = gp.Game.Developer.Id,
                            Name = gp.Game.Developer.Name,
                            Location = gp.Game.Developer.Location
                        }
                    }).ToList()
                })
                .ToListAsync();
        }

        // GET: api/Platforms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlatformResponseDTO>> GetPlatform(int id)
        {
            var platform = await _context.Platform
                .Include(p => p.GamePlatforms)
                    .ThenInclude(gp => gp.Game)
                        .ThenInclude(g => g.Genre)
                .Include(p => p.GamePlatforms)
                    .ThenInclude(gp => gp.Game)
                        .ThenInclude(g => g.Developer)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (platform == null)
            {
                return NotFound();
            }

            var platformDto = new PlatformResponseDTO
            {
                Id = platform.Id,
                Name = platform.Name,
                Games = platform.GamePlatforms.Select(gp => new GameResponseDTO
                {
                    Id = gp.Game.Id,
                    Name = gp.Game.Name,
                    Description = gp.Game.Description,
                    ReleaseDate = gp.Game.ReleaseDate,
                    Genre = new GenreDTO
                    {
                        Id = gp.Game.Genre.Id,
                        Name = gp.Game.Genre.Name
                    },
                    Developer = new DeveloperDTO
                    {
                        Id = gp.Game.Developer.Id,
                        Name = gp.Game.Developer.Name,
                        Location = gp.Game.Developer.Location
                    }
                }).ToList()
            };

            return platformDto;
        }

        // PUT: api/Platforms/5
        [HttpPut("{id}")]
        public async Task<ActionResult<PlatformResponseDTO>> PutPlatform(int id, InputPlatformDTO inputPlatformDto)
        {
            try
            {
                var platform = await _context.Platform
                    .Include(p => p.GamePlatforms)
                        .ThenInclude(gp => gp.Game)
                            .ThenInclude(g => g.Genre)
                    .Include(p => p.GamePlatforms)
                        .ThenInclude(gp => gp.Game)
                            .ThenInclude(g => g.Developer)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (platform == null)
                {
                    return NotFound($"Platform with ID {id} does not exist.");
                }

                platform.Name = inputPlatformDto.Name;
                await _context.SaveChangesAsync();

                // Return updated platform with all related data
                return Ok(new PlatformResponseDTO
                {
                    Id = platform.Id,
                    Name = platform.Name,
                    Games = platform.GamePlatforms.Select(gp => new GameResponseDTO
                    {
                        Id = gp.Game.Id,
                        Name = gp.Game.Name,
                        Description = gp.Game.Description,
                        ReleaseDate = gp.Game.ReleaseDate,
                        Genre = new GenreDTO
                        {
                            Id = gp.Game.Genre.Id,
                            Name = gp.Game.Genre.Name
                        },
                        Developer = new DeveloperDTO
                        {
                            Id = gp.Game.Developer.Id,
                            Name = gp.Game.Developer.Name,
                            Location = gp.Game.Developer.Location
                        }
                    }).ToList()
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlatformExists(id))
                {
                    return NotFound($"Platform with ID {id} was deleted by another request.");
                }
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the platform: {ex.Message}");
            }
        }

        // POST: api/Platforms
        [HttpPost]
        public async Task<ActionResult<PlatformResponseDTO>> PostPlatform(InputPlatformDTO inputPlatformDto)
        {
            try
            {
                var platform = new Platform
                {
                    Name = inputPlatformDto.Name
                };

                _context.Platform.Add(platform);
                await _context.SaveChangesAsync();

                // Return the created platform with empty games list
                return CreatedAtAction(
                    nameof(GetPlatform),
                    new { id = platform.Id },
                    new PlatformResponseDTO
                    {
                        Id = platform.Id,
                        Name = platform.Name,
                        Games = new List<GameResponseDTO>()
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the platform: {ex.Message}");
            }
        }

        // DELETE: api/Platforms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlatform(int id)
        {
            var platform = await _context.Platform.FindAsync(id);
            if (platform == null)
            {
                return NotFound();
            }

            _context.Platform.Remove(platform);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlatformExists(int id)
        {
            return _context.Platform.Any(e => e.Id == id);
        }
    }
}
