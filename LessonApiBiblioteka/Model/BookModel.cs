using LessonApiBiblioteka.Controllers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LessonApiBiblioteka.Model
{
    public class BookModel
    {
        [Key]
        public int Id_Book { get; set; }
        public string Title_Book { get; set; }
        public int YearPublished_Book { get; set; }
        public string Description_Book { get; set; }
        public int AvailableCopies { get; set; }
        [ForeignKey("Genre")]
        public int Genre_Id { get; set; }
        public GenreModel Genre { get; set; }

        [ForeignKey("Author")]
        public int Author_Id { get; set; }
        public AuthorModel Author { get; set; }
    } 
}
