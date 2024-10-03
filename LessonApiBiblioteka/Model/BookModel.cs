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
        [ForeignKey("GenreModel")]
        public int Genre_Id;
        public GenreModel Genre { get; set; }
        [ForeignKey("AuthorModel")]
        public int Author_Id;
        public AuthorModel Author { get; set; }
        
    }
}
