using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LessonApiBiblioteka.Requests;
using LessonApiBiblioteka.DataBaseContext;
using LessonApiBiblioteka.Model;

[ApiController]
[Route("api/[controller]")]
public class BookRentalController : ControllerBase
{
    private readonly LibraryApiDb _context;

    public BookRentalController(LibraryApiDb context)
    {
        _context = context;
    }

    [HttpPost("rent")]
    public async Task<IActionResult> RentBook([FromQuery] RentBookRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var book = await _context.Book.FindAsync(request.BookId);
            if (book == null)
            {
                return NotFound("Книга не найдена.");
            }

            if (book.AvailableCopies <= 0)
            {
                return BadRequest("Нет доступных копий для аренды.");
            }
            var reader = await _context.Reader
        .FirstOrDefaultAsync(g => g.Id_Reader == request.UserId);
            var rental = new BookRentalModel
            {
                BookId = request.BookId,
                UserId = reader.Id_Reader,
                RentalDate = DateTime.UtcNow,
                DueDate = request.DueDate
            };

            book.AvailableCopies--;

            _context.Rental.Add(rental);
            await _context.SaveChangesAsync();

            return Ok("Книга арендована.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка сервера: {ex.Message}");
        }
    }

    [HttpPost("return")]
    public async Task<IActionResult> ReturnBook([FromQuery] ReturnBookRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var rental = await _context.Rental.FindAsync(request.RentalId);
            if (rental == null)
            {
                return NotFound("Аренда не найдена.");
            }

            var book = await _context.Book.FindAsync(rental.BookId);
            if (book == null)
            {
                return NotFound("Книга не найдена.");
            }

            rental.ReturnDate = DateTime.UtcNow;
            book.AvailableCopies++; 

            await _context.SaveChangesAsync();

            return Ok("Книга возвращена.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка сервера: {ex.Message}");
        }
    }

    [HttpGet("user/{userId}/history")]
    public async Task<IActionResult> GetRentalHistoryByUser(int userId)
    {
        try
        {
            var rentals = await _context.Rental
                .Where(r => r.UserId == userId)
                .Include(r => r.Book)
                .Include(r => r.Reader)
                .Select(r => new
                {
                    BookTitle = r.Book.Title_Book,
                    UserName = r.Reader.FirstName + " " + r.Reader.LastName,
                    RentalDate = r.RentalDate,
                    DueDate = r.DueDate,
                    ReturnDate = r.ReturnDate
                })
                .ToListAsync();

            if (rentals == null || rentals.Count == 0)
            {
                return NotFound("История аренды не найдена.");
            }

            return Ok(rentals);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка сервера: {ex.Message}");
        }
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentRentals()
    {
        try
        {
            var rentals = await _context.Rental
                .Where(r => r.ReturnDate == null)
                .Include(r => r.Book)
                .Include(r => r.Reader)
                .Select(r => new
                {
                    BookTitle = r.Book.Title_Book,
                    UserName = r.Reader.FirstName + " " + r.Reader.LastName,
                    RentalDate = r.RentalDate,
                    DueDate = r.DueDate
                })
                .ToListAsync();

            return Ok(rentals);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка сервера: {ex.Message}");
        }
    }

    [HttpGet("book/{bookId}/history")]
    public async Task<IActionResult> GetRentalHistoryByBook(int bookId)
    {
        try
        {
            var rentals = await _context.Rental
                .Where(r => r.BookId == bookId)
                .Include(r => r.Book)
                .Include(r => r.Reader)
                .Select(r => new
                {
                    BookTitle = r.Book.Title_Book,
                    UserName = r.Reader.FirstName + " " + r.Reader.LastName,
                    RentalDate = r.RentalDate,
                    DueDate = r.DueDate,
                    ReturnDate = r.ReturnDate
                })
                .ToListAsync();

            if (rentals == null || rentals.Count == 0)
            {
                return NotFound("История аренды для книги не найдена.");
            }

            return Ok(rentals);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка сервера: {ex.Message}");
        }
    }
}