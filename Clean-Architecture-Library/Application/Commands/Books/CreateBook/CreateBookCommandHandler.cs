//using Domain.Entities;
//using Infrastructure;
//using MediatR;

//namespace Application.Commands.Books.CreateBook
//{
//    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Book>
//    {
//        private readonly FakeDatabase _fakeDatabase;

//        public CreateBookCommandHandler(FakeDatabase fakeDatabase)
//        {
//            _fakeDatabase = fakeDatabase;
//        }

//        public Task<Book> Handle(CreateBookCommand request, CancellationToken cancellationToken)
//        {
//            if (_fakeDatabase.Authors.All(a => a.Id != request.AuthorId))
//            {
//                throw new KeyNotFoundException($"Author with ID {request.AuthorId} does not exist.");
//            }

//            var newBook = new Book(
//                _fakeDatabase.Books.Count + 1,
//                request.Title,
//                request.Description,
//                request.AuthorId
//            );

//            _fakeDatabase.Books.Add(newBook);

//            return Task.FromResult(newBook);
//        }
//    }
//}
