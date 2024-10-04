using System.ComponentModel.DataAnnotations;

namespace LessonApiBiblioteka.Requests
{
    public class ReaderRequest
    {
        [Required(ErrorMessage = "Имя обязательно.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Контактные данные обязательны.")]
        public string ContactInfo { get; set; }
    }
}
