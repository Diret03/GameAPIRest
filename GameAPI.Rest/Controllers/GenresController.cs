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
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            var genre = await _context.Genre.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return genre;
        }

        // PUT: api/Genres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, Genre genre)
        {
            if (id != genre.Id)
            {
                return BadRequest();
            }

            _context.Entry(genre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
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

        // POST: api/Genres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(Genre genre)
        {
            _context.Genre.Add(genre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenre", new { id = genre.Id }, genre);
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
