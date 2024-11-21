using Domain.Entities;
using MediatR;

namespace Application.Commands.Books.CreateBook
{
    public class CreateBookCommand : IRequest<Book>
    {
        public string Title { get; }
        public string Description { get; }
        public int AuthorId { get; }

        public CreateBookCommand(string title, string description, int authorId)
        {
            Title = title;
            Description = description;
            AuthorId = authorId;
        }
    }
}
