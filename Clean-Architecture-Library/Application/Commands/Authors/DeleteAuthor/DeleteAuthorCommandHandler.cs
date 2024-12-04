//using Infrastructure;
//using MediatR;

//namespace Application.Commands.Authors.DeleteAuthor
//{
//    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
//    {
//        private readonly FakeDatabase _fakeDatabase;

//        public DeleteAuthorCommandHandler(FakeDatabase fakeDatabase)
//        {
//            _fakeDatabase = fakeDatabase;
//        }

//        public Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
//        {
//            var author = _fakeDatabase.Authors.FirstOrDefault(a => a.Id == request.Id);

//            if (author == null)
//            {
//                throw new KeyNotFoundException($"Author with ID {request.Id} not found.");
//            }

//            bool associatedBooks = _fakeDatabase.Books.Any(b => b.AuthorId == request.Id);

//            if (associatedBooks)
//            {
//                throw new InvalidOperationException($"Cannot delete author with ID {request.Id}, they are associated with existing books.");
//            }

//            _fakeDatabase.Authors.Remove(author);

//            return Task.FromResult(true);
//        }
//    }
//}
