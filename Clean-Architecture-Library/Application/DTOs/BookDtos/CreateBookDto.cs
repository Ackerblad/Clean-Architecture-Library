namespace Application.DTOs.BookDtos
{
    public class CreateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
    }
}
