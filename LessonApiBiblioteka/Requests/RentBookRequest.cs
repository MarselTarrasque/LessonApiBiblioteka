using System.ComponentModel.DataAnnotations;

namespace LessonApiBiblioteka.Requests
{
    public class RentBookRequest
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
