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
    public class DevelopersController : ControllerBase
    {
        private readonly GameAPIRestDbContext _context;

        public DevelopersController(GameAPIRestDbContext context)
        {
            _context = context;
        }

        // GET: api/Developers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeveloperResponseDTO>>> GetDeveloper()
        {
            return await _context.Developer
                    .Include(d => d.Games)
                        .ThenInclude(g => g.Genre)
                    .Include(d => d.Games)
                        .ThenInclude(g => g.GamePlatforms)
                            .ThenInclude(gp => gp.Platform)
                    .Select(dev => new DeveloperResponseDTO
                    {
                        Id = dev.Id,
                        Name = dev.Name,
                        Location = dev.Location,
                        Games = dev.Games.Select(g => new GameResponseDTO
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
                            Platforms = g.GamePlatforms.Select(gp => new PlatformDTO
                            {
                                Id = gp.Platform.Id,
                                Name = gp.Platform.Name
                            }).ToList()
                        }).ToList()
                    })
                    .ToListAsync();
        }

        // GET: api/Developers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeveloperResponseDTO>> GetDeveloper(int id)
        {
            var developer = await _context.Developer
                .Include(d => d.Games)
                    .ThenInclude(g => g.Genre)
                .Include(d => d.Games)
                    .ThenInclude(g => g.GamePlatforms)
                        .ThenInclude(gp => gp.Platform)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (developer == null)
            {
                return NotFound();
            }

            var developerDto = new DeveloperResponseDTO
            {
                Id = developer.Id,
                Name = developer.Name,
                Location = developer.Location,
                Games = developer.Games.Select(g => new GameResponseDTO
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
                    Platforms = g.GamePlatforms.Select(gp => new PlatformDTO
                    {
                        Id = gp.Platform.Id,
                        Name = gp.Platform.Name
                    }).ToList()
                }).ToList()
            };

            return developerDto;
        }

        // PUT: api/Developers/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DeveloperResponseDTO>> PutDeveloper(int id, UpdateDeveloperDTO updateDeveloperDto)
        {
            try
            {
                var developer = await _context.Developer
                    .Include(d => d.Games)
                        .ThenInclude(g => g.Genre)
                    .Include(d => d.Games)
                        .ThenInclude(g => g.GamePlatforms)
                            .ThenInclude(gp => gp.Platform)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (developer == null)
                {
                    return NotFound($"Developer with ID {id} does not exist.");
                }

                // Only update properties that are provided
                if (updateDeveloperDto.Name != null)
                {
                    developer.Name = updateDeveloperDto.Name;
                }

                if (updateDeveloperDto.Location != null)
                {
                    developer.Location = updateDeveloperDto.Location;
                }

                await _context.SaveChangesAsync();

                // Return the updated developer with all related data
                return Ok(new DeveloperResponseDTO
                {
                    Id = developer.Id,
                    Name = developer.Name,
                    Location = developer.Location,
                    Games = developer.Games.Select(g => new GameResponseDTO
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
                        Platforms = g.GamePlatforms.Select(gp => new PlatformDTO
                        {
                            Id = gp.Platform.Id,
                            Name = gp.Platform.Name
                        }).ToList()
                    }).ToList()
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeveloperExists(id))
                {
                    return NotFound($"Developer with ID {id} was deleted by another request.");
                }
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the developer: {ex.Message}");
            }
        }

        // POST: api/Developers
        [HttpPost]
        public async Task<ActionResult<DeveloperResponseDTO>> PostDeveloper(CreateDeveloperDTO createDeveloperDto)
        {
            try
            {
                var developer = new Developer
                {
                    Name = createDeveloperDto.Name,
                    Location = createDeveloperDto.Location
                };

                _context.Developer.Add(developer);
                await _context.SaveChangesAsync();

                // Return the created developer with empty games list
                return CreatedAtAction(
                    nameof(GetDeveloper),
                    new { id = developer.Id },
                    new DeveloperResponseDTO
                    {
                        Id = developer.Id,
                        Name = developer.Name,
                        Location = developer.Location,
                        Games = new List<GameResponseDTO>()
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the developer: {ex.Message}");
            }
        }

        // DELETE: api/Developers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeveloper(int id)
        {
            var developer = await _context.Developer.FindAsync(id);
            if (developer == null)
            {
                return NotFound();
            }

            _context.Developer.Remove(developer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeveloperExists(int id)
        {
            return _context.Developer.Any(e => e.Id == id);
        }
    }
}
