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
    public class GenresController : ControllerBase
    {
        private readonly GameAPIRestDbContext _context;

        public GenresController(GameAPIRestDbContext context)
        {
            _context = context;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenre()
        {
            //return await _context.Genre
            //    .Include(g => g.Games)
            //    .ToListAsync();

            return await _context.Genre
                   .Include(g => g.Games)
                       .ThenInclude(game => game.Developer)
                   .Include(g => g.Games)
                       .ThenInclude(game => game.GamePlatforms)
                           .ThenInclude(gp => gp.Platform)
                   .Select(genre => new GenreDTO
                   {
                       Id = genre.Id,
                       Name = genre.Name,
                       Games = genre.Games.Select(game => new GameResponseDTO
                       {
                           Id = game.Id,
                           Name = game.Name,
                           Description = game.Description,
                           ReleaseDate = game.ReleaseDate,
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
                       }).ToList()
                   })
                   .ToListAsync();
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreResponseDTO>> GetGenre(int id)
        {
            var genre = await _context.Genre
                .Include(g => g.Games)
                    .ThenInclude(game => game.Developer)
                .Include(g => g.Games)
                    .ThenInclude(game => game.GamePlatforms)
                        .ThenInclude(gp => gp.Platform)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            return new GenreResponseDTO
            {
                Id = genre.Id,
                Name = genre.Name,
                Games = genre.Games.Select(g => new GameResponseDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    ReleaseDate = g.ReleaseDate,
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
                }).ToList()
            };
        }

        // PUT: api/Genres/5
        [HttpPut("{id}")]
        public async Task<ActionResult<GenreDTO>> PutGenre(int id, UpdateGenreDTO updateGenreDto)
        {
            try
            {
                var genre = await _context.Genre
                    .Include(g => g.Games)
                        .ThenInclude(game => game.Developer)
                    .Include(g => g.Games)
                        .ThenInclude(game => game.GamePlatforms)
                            .ThenInclude(gp => gp.Platform)
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (genre == null)
                {
                    return NotFound($"Genre with ID {id} does not exist.");
                }

                // Only update name if provided
                if (updateGenreDto.Name != null)
                {
                    genre.Name = updateGenreDto.Name;
                }

                await _context.SaveChangesAsync();

                // Return updated genre with games
                return Ok(new GenreDTO
                {
                    Id = genre.Id,
                    Name = genre.Name,
                    Games = genre.Games.Select(game => new GameResponseDTO
                    {
                        Id = game.Id,
                        Name = game.Name,
                        Description = game.Description,
                        ReleaseDate = game.ReleaseDate,
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
                    }).ToList()
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
                {
                    return NotFound($"Genre with ID {id} was deleted by another request.");
                }
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the genre: {ex.Message}");
            }
        }

        // POST: api/Genres
        [HttpPost]
        public async Task<ActionResult<GenreDTO>> PostGenre(CreateGenreDTO createGenreDto)
        {
            try
            {
                var genre = new Genre
                {
                    Name = createGenreDto.Name
                };

                _context.Genre.Add(genre);
                await _context.SaveChangesAsync();

                // Return the created genre with empty games list
                return CreatedAtAction(
                    nameof(GetGenre),
                    new { id = genre.Id },
                    new GenreDTO
                    {
                        Id = genre.Id,
                        Name = genre.Name,
                        Games = new List<GameResponseDTO>()
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the genre: {ex.Message}");
            }
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genre.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            _context.Genre.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(int id)
        {
            return _context.Genre.Any(e => e.Id == id);
        }
    }
}
