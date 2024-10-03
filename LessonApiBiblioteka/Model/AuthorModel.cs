using System.ComponentModel.DataAnnotations;

namespace LessonApiBiblioteka.Model
{
  public class AuthorModel
    {
        [Key]
        public int Id_Author { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<BookModel> Books { get; set; }
    }
}
