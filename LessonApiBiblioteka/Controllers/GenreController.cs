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
            try
            {
                var genres = await _context.Genre.ToListAsync();
                return Ok(genres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromQuery] GenreRequest newGenre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var genre = new GenreModel
                {
                    Name = newGenre.Name
                };

                _context.Genre.Add(genre);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAllGenres), new { id = genre.Id_Genre }, genre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromQuery] GenreRequest updateGenre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var genre = await _context.Genre.FindAsync(id);
                if (genre == null)
                {
                    return NotFound("Жанр с указанным идентификатором не найден.");
                }

                genre.Name = updateGenre.Name;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre( int id)
        {
            try
            {
                var genre = await _context.Genre.FindAsync(id);
                if (genre == null)
                {
                    return NotFound("Жанр с указанным идентификатором не найден.");
                }

                _context.Genre.Remove(genre);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
}
