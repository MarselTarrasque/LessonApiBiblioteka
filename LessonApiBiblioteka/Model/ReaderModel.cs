using System.ComponentModel.DataAnnotations;

namespace LessonApiBiblioteka.Model
{
    public class ReaderModel
    {
        [Key]
        public int Id_Reader { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string ContactInfo { get; set; }
    }
}
