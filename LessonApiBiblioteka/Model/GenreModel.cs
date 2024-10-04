using System.ComponentModel.DataAnnotations;

namespace LessonApiBiblioteka.Model
{
    public class GenreModel
    {
        [Key]
        public int Id_Genre { get; set; }
        public string Name { get; set; }
    }
}
