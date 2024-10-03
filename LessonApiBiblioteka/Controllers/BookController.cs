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
    public class BookController : Controller
    {
        readonly LibraryApiDb _context;

        public BookController(LibraryApiDb context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _context.Book.ToListAsync();
            return new OkObjectResult(new
            {
                books = books,
                status = true
            });
        }

        [HttpPost]
        [Route("createNewBook")]
        public async Task<IActionResult> CreateNewBook(CreateNewBookRequest newBook)
        {
            var book = new BookModel
            {
                Title_Book = newBook.Title_Book,
                YearPublished_Book = newBook.YearPublished_Book,
                Description_Book = newBook.Description_Book,
                AvailableCopies = newBook.AvailableCopies,
                Genre_Id = newBook.Genre_Id,
                Author_Id = newBook.Author_Id
            };

            _context.Book.Add(book);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id_Book }, book);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookRequest updateBook)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            book.Title_Book = updateBook.Title_Book;
            book.YearPublished_Book = updateBook.YearPublished_Book;
            book.Description_Book = updateBook.Description_Book;
            book.AvailableCopies = updateBook.AvailableCopies;
            book.Genre_Id = updateBook.Genre_Id;
            book.Author_Id = updateBook.Author_Id;

            _context.Book.Update(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("byGenre/{genreId}")]
        public async Task<IActionResult> GetBooksByGenre(int genreId)
        {
            var books = await _context.Book
                .Where(b => b.Genre_Id == genreId)
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks(string query)
        {
            var books = await _context.Book
                .Where(b => b.Title_Book.Contains(query) || b.Author.FirstName.Contains(query))
                .ToListAsync();

            return Ok(books);
        }
    }
}
