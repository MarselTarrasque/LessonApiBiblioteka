using LessonApiBiblioteka.DataBaseContext;
using LessonApiBiblioteka.Model;
using LessonApiBiblioteka.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LessonApiBiblioteka.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly LibraryApiDb _context;

        public GenreController(LibraryApiDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _context.Genre.ToListAsync();
            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreById(int id)
        {
            var genre = await _context.Genre.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre(CreateNewGenreRequest newGenre)
        {
            var genre = new GenreModel
            {
                Name = newGenre.Name
            };

            _context.Genre.Add(genre);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGenreById), new { id = genre.Id_Genre }, genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, UpdateGenreRequest updateGenre)
        {
            var genre = await _context.Genre.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            genre.Name = updateGenre.Name;

            await _context.SaveChangesAsync();

            return NoContent();
        }

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
    }
}
