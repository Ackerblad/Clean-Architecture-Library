using Domain.Entities;
using Infrastructure;
using MediatR;

namespace Application.Commands.Books.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Book>
    {
        private readonly FakeDatabase _fakeDatabase;

        public UpdateBookCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<Book> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = _fakeDatabase.Books.FirstOrDefault(b => b.Id == request.Id);

            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {request.Id} not found.");
            }

            if (request.AuthorId.HasValue && !_fakeDatabase.Authors.Any(a => a.Id == request.AuthorId.Value))
            {
                throw new KeyNotFoundException($"Author with ID {request.AuthorId.Value} not found.");
            }

            if (request.Title != null) book.Title = request.Title;
            if (request.Description != null) book.Description = request.Description;
            if (request.AuthorId.HasValue) book.AuthorId = request.AuthorId.Value;

            return Task.FromResult(book);
        }
    }
}
