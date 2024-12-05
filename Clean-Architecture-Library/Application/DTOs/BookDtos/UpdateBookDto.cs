namespace Application.DTOs.BookDtos
{
    public class UpdateBookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
    }
}
