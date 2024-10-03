namespace LessonApiBiblioteka.Requests
{
    public class CreateNewBookRequest
    {
        public string Title_Book { get; set; }
        public int YearPublished_Book { get; set; }
        public string Description_Book { get; set; }
        public int AvailableCopies { get; set; }
        public int Genre_Id { get; set; }
        public int Author_Id { get; set; }
    }
}
