namespace Application.DTOs.BookDtos
{
    public class UpdateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
    }
}
