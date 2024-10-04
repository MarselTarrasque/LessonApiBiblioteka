using System.ComponentModel.DataAnnotations;

namespace LessonApiBiblioteka.Requests
{
    public class BookRequest
    {
        [Required(ErrorMessage = "Название книги обязательно.")]
        public string Title_Book { get; set; }

        [Required(ErrorMessage = "Год издания обязателен.")]
        public int YearPublished_Book { get; set; }

        public string Description_Book { get; set; }

        [Required(ErrorMessage = "Количество доступных копий обязательно.")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество доступных копий не может быть отрицательным.")]
        public int AvailableCopies { get; set; }

        [Required(ErrorMessage = "Идентификатор жанра обязателен.")]
        public int Genre_Id { get; set; }

        [Required(ErrorMessage = "Идентификатор автора обязателен.")]
        public int Author_Id { get; set; }
    }
}
