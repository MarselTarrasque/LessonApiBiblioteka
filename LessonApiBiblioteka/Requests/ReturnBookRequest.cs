using System.ComponentModel.DataAnnotations;

namespace LessonApiBiblioteka.Requests
{
    public class ReturnBookRequest
    {
        [Required]
        public int RentalId { get; set; }
    }
}
