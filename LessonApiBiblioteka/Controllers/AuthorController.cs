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
    public class AuthorController : ControllerBase
    {
        private readonly LibraryApiDb _context;

        public AuthorController(LibraryApiDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _context.Author.ToListAsync();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _context.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor(AuthorModel author)
        {
            _context.Author.Add(author);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id_Author }, author);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, UpdateAuthorRequest updatedAuthor)
        {
            if (id != updatedAuthor.Id_Author)
            {
                return BadRequest();
            }

            _context.Entry(updatedAuthor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Author.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
