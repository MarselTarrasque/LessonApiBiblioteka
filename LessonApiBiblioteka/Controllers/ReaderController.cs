using LessonApiBiblioteka.DataBaseContext;
using LessonApiBiblioteka.Model;
using LessonApiBiblioteka.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace LessonApiBiblioteka.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderController : ControllerBase
    {
        readonly LibraryApiDb _context;

        public ReaderController(LibraryApiDb context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterReader([FromQuery] ReaderRequest readerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var reader = new ReaderModel
                {
                    FirstName = readerRequest.FirstName,
                    LastName = readerRequest.LastName,
                    DateOfBirth = readerRequest.DateOfBirth,
                    ContactInfo = readerRequest.ContactInfo
                };

                _context.Reader.Add(reader);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetReaderById), new { id = reader.Id_Reader }, reader);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReaders()
        {
            try
            {
                var readers = await _context.Reader.ToListAsync();
                return Ok(readers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReaderById(  int id)
        {
            try
            {
                var reader = await _context.Reader.FindAsync(id);
                if (reader == null)
                {
                    return NotFound("Читатель с указанным идентификатором не найден.");
                }
                return Ok(reader);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReader(int id, [FromQuery] ReaderRequest readerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var reader = await _context.Reader.FindAsync(id);
                if (reader == null)
                {
                    return NotFound("Читатель с указанным идентификатором не найден.");
                }

                reader.FirstName = readerRequest.FirstName;
                reader.LastName = readerRequest.LastName;
                reader.DateOfBirth = readerRequest.DateOfBirth;
                reader.ContactInfo = readerRequest.ContactInfo;

                _context.Reader.Update(reader);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReader( int id)
        {
            try
            {
                var reader = await _context.Reader.FindAsync(id);
                if (reader == null)
                {
                    return NotFound("Читатель с указанным идентификатором не найден.");
                }

                _context.Reader.Remove(reader);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
        [HttpGet("rented-books/by-name")]
        public async Task<IActionResult> GetRentedBooksByReaderName([FromQuery] string firstName, [FromQuery] string lastName)
        {
            try
            {
                var reader = await _context.Reader
                    .FirstOrDefaultAsync(r => r.FirstName == firstName && r.LastName == lastName);

                if (reader == null)
                {
                    return NotFound("Читатель с указанным именем и фамилией не найден.");
                }

                var rentedBooks = await _context.Rental
                    .Where(br => br.UserId == reader.Id_Reader && br.ReturnDate == null)
                    .Include(br => br.Book)
                    .Select(br => new
                    {
                        br.Book.Id_Book,
                        br.Book.Title_Book,
                        br.RentalDate,
                        br.DueDate
                    })
                    .ToListAsync();

                if (rentedBooks == null || rentedBooks.Count == 0)
                {
                    return Ok("Нет текущих арендованных книг.");
                }

                return Ok(rentedBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
}
