using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LessonApiBiblioteka.Model
{
    public class BookRentalModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }
        [Required]
        [ForeignKey("Reader")]
        public int UserId { get; set; }
        [Required]
        public DateTime RentalDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BookModel Book { get; set; }
        public ReaderModel Reader { get; set; }
    }
}
