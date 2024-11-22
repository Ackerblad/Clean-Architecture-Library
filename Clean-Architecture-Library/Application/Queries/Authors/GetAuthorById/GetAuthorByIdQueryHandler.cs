using Domain.Entities;
using Infrastructure;
using MediatR;

namespace Application.Queries.Authors.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Author>
    {
        private readonly FakeDatabase _fakeDatabase;

        public GetAuthorByIdQueryHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var author = _fakeDatabase.Authors.FirstOrDefault(a => a.Id == request.Id);

            if (author == null)
            {
                throw new KeyNotFoundException($"Author with ID {request.Id} not found.");
            }

            return Task.FromResult(author);
        }
    }
}
