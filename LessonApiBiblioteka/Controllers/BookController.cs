using LessonApiBiblioteka.DataBaseContext;
using LessonApiBiblioteka.Model;
using LessonApiBiblioteka.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

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
        public class AllBooksClassId
        {
            public int Id_Books { get; set; }
            public string Title { get; set; }
            public int AvailableCopies { get; set; }
            public int YearOfPublication { get; set; }
            public string Description { get; set; }
            public int Id_Autors { get; set; }
            public int Id_Genre { get; set; }

        }
        public class AllBooksClassName
        {
            public int Id_Books { get; set; }
            public string Title { get; set; }
            public int AvailableCopies { get; set; }
            public int YearOfPublication { get; set; }
            public string Description { get; set; }
            public string Name_Author { get; set; }
            public string Name_Genre { get; set; }

        }
        public class BookAvailableCopies
        {
            public int Id_Books { get; set; }
            public string Title { get; set; }
            public int AvailableCopies { get; set; }

        }

        [HttpGet("getAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _context.Book.ToListAsync();

                if (books == null || books.Count == 0)
                {
                    return NotFound("Книги не найдены.");
                }

                var booksDto = books.Select(b => new AllBooksClassId
                {
                    Id_Books = b.Id_Book,
                    Title = b.Title_Book,
                    AvailableCopies = b.AvailableCopies,
                    YearOfPublication = b.YearPublished_Book,
                    Description = b.Description_Book,
                    Id_Autors = b.Author_Id,
                    Id_Genre = b.Genre_Id,
                });

                return Ok(booksDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Недопустимый запрос: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"Ошибка операции: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Идентификатор книги должен быть положительным числом.");
                }

                var book = await _context.Book.FindAsync(id);
                if (book == null)
                {
                    return NotFound("Книга с указанным идентификатором не найдена.");
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }


        [HttpPost("createNewBook")]
        public async Task<IActionResult> CreateNewBook([Required] [FromQuery] BookRequest newBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Некорректные данные для создания книги.");
            }

            try
            {
                var genre = await _context.Genre.FirstOrDefaultAsync(g => g.Id_Genre == newBook.Genre_Id);
                if (genre == null)
                {
                    return NotFound("Указанный жанр не найден.");
                }

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

                var bookDto = new AllBooksClassId
                {
                    Id_Books = book.Id_Book,
                    Title = book.Title_Book,
                    AvailableCopies = book.AvailableCopies,
                    YearOfPublication = book.YearPublished_Book,
                    Description = book.Description_Book,
                    Id_Autors = book.Author_Id,
                    Id_Genre = book.Genre_Id,
                };

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id_Book }, bookDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook([Required] int id, [FromQuery] BookRequest updateBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Некорректные данные для обновления книги.");
            }

            try
            {
                var book = await _context.Book.FindAsync(id);
                if (book == null)
                {
                    return NotFound("Книга с указанным идентификатором не найдена.");
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromQuery] int id)
        {
            try
            {
                var book = await _context.Book.FindAsync(id);
                if (book == null)
                {
                    return NotFound("Книга с указанным идентификатором не найдена.");
                }

                _context.Book.Remove(book);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("byGenreId/{genreId}")]
        public async Task<IActionResult> GetBooksByGenreId([Required] [FromRoute] int genreId)
        {
            try
            {
                var books = await _context.Book
                    .Include(b => b.Author)
                    .Include(b => b.Genre)
                    .Where(b => b.Genre_Id == genreId)
                    .ToListAsync();

                if (books == null || books.Count == 0)
                {
                    return NotFound("Книги с указанным жанром не найдены.");
                }

                var booksDto = books.Select(b => new AllBooksClassName
                {
                    Id_Books = b.Id_Book,
                    Title = b.Title_Book,
                    AvailableCopies = b.AvailableCopies,
                    YearOfPublication = b.YearPublished_Book,
                    Description = b.Description_Book,
                    Name_Author = $"{b.Author.LastName} {b.Author.FirstName}",
                    Name_Genre = b.Genre.Name,
                });

                return Ok(booksDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("byGenreName/{genreName}")]
        public async Task<IActionResult> GetBooksByGenreName([Required] [FromQuery] string genreName)
        {
            try
            {
                var genre = await _context.Genre
                    .FirstOrDefaultAsync(g => g.Name.Equals(genreName, StringComparison.OrdinalIgnoreCase));

                if (genre == null)
                {
                    return NotFound($"Жанр с названием '{genreName}' не найден.");
                }

                var books = await _context.Book
                    .Include(b => b.Author)
                    .Include(b => b.Genre)
                    .Where(b => b.Genre_Id == genre.Id_Genre)
                    .ToListAsync();

                if (books == null || books.Count == 0)
                {
                    return NotFound("Книги с указанным жанром не найдены.");
                }

                var booksDto = books.Select(b => new AllBooksClassName
                {
                    Id_Books = b.Id_Book,
                    Title = b.Title_Book,
                    AvailableCopies = b.AvailableCopies,
                    YearOfPublication = b.YearPublished_Book,
                    Description = b.Description_Book,
                    Name_Author = $"{b.Author.LastName} {b.Author.FirstName}",
                    Name_Genre = b.Genre.Name,
                });

                return Ok(booksDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([Required] [FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Запрос для поиска не может быть пустым.");
            }

            try
            {
                var books = await _context.Book.Include(b => b.Author).Include(b => b.Genre)
                    .Where(b => b.Title_Book.Contains(query) || b.Author.FirstName.Contains(query) || b.Author.LastName.Contains(query) || b.Genre.Name.Contains(query))
                    .ToListAsync();

                if (books == null || books.Count == 0)
                {
                    return NotFound("Книги по вашему запросу не найдены.");
                }

                var booksDto = books.Select(b => new AllBooksClassName
                {
                    Id_Books = b.Id_Book,
                    Title = b.Title_Book,
                    Name_Author = $"{b.Author.LastName} {b.Author.FirstName}",
                    Name_Genre = b.Genre.Name,
                    AvailableCopies = b.AvailableCopies,
                    YearOfPublication = b.YearPublished_Book,
                    Description = b.Description_Book
                });

                return Ok(booksDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("{titleBook}/available-copies")]
        public async Task<IActionResult> GetAvailableCopies([Required] string titleBook)
        {
            if (string.IsNullOrWhiteSpace(titleBook))
            {
                return BadRequest("Название книги обязательно для поиска.");
            }

            try
            {
                var books = await _context.Book
                    .Where(b => b.Title_Book.Contains(titleBook))
                    .Select(b => new BookAvailableCopies
                    {
                        Id_Books = b.Id_Book,
                        Title = b.Title_Book,
                        AvailableCopies = b.AvailableCopies
                    })
                    .ToListAsync();

                if (books == null || books.Count == 0)
                {
                    return NotFound("Книги с указанным названием не найдены.");
                }

                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
}
