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
        public class AuthorRequestWithoutId
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            // Нет поля ID
        }
        readonly LibraryApiDb _context;

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
        public async Task<IActionResult> CreateAuthor([FromQuery] AuthorRequestWithoutId newAuthor)
        {
            var autors = new AuthorModel()
            {
                FirstName = newAuthor.FirstName,
                LastName = newAuthor.LastName,
            };

            await _context.Author.AddAsync(autors);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Автор успешно создан",
                FirstName = autors.FirstName,
                LastName = autors.LastName
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorRequest updatedAuthor)
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
