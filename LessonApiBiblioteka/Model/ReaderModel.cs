using System.ComponentModel.DataAnnotations;

namespace LessonApiBiblioteka.Model
{
    public class ReaderModel
    {
        [Key]
        public int Id_Reader { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ContactInfo { get; set; }

        // Связь с арендованными книгами
        public ICollection<RentalModel> Rentals { get; set; }
    }
}
