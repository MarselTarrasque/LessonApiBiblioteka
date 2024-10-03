using LessonApiBiblioteka.Model;
using Microsoft.EntityFrameworkCore;

namespace LessonApiBiblioteka.DataBaseContext
{
    public class LibraryApiDb : DbContext
    {
        public LibraryApiDb(DbContextOptions options) : base(options)
        {

        }
        public DbSet<BookModel> Book { get; set; }
        public DbSet<AuthorModel> Author { get; set; }
        public DbSet<GenreModel> Genre { get; set; }
    }
}
