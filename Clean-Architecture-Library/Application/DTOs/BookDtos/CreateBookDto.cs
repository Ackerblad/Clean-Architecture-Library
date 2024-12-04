namespace Application.DTOs.BookDtos
{
    public class CreateBookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
    }
}
