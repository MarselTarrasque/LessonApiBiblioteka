using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LessonApiBiblioteka.Model
{
    public class RentalModel
    {
        [Key]
        public int Id_Rental { get; set; }

        [ForeignKey("BookModel")]
        public int Book_Id { get; set; }
        public BookModel Book { get; set; }

        [ForeignKey("ReaderModel")]
        public int Reader_Id { get; set; }
        public ReaderModel Reader { get; set; }

        public DateTime RentalStartDate { get; set; }
        public DateTime RentalEndDate { get; set; }
        public bool IsReturned { get; set; }
    }
}
