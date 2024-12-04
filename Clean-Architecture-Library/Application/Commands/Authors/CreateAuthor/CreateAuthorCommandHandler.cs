//using Domain.Entities;
//using Infrastructure;
//using MediatR;

//namespace Application.Commands.Authors.CreateAuthor
//{
//    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Author>
//    {
//        private readonly FakeDatabase _fakeDatabase;

//        public CreateAuthorCommandHandler(FakeDatabase fakeDatabase)
//        {
//            _fakeDatabase = fakeDatabase;
//        }

//        public Task<Author> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
//        {
//            if (_fakeDatabase.Authors.Any(a => a.FirstName == request.FirstName && a.LastName == request.LastName))
//            {
//                throw new InvalidOperationException($"An author with the name '{request.FirstName} {request.LastName}' already exists.");
//            }

//            var newAuthor = new Author(
//                _fakeDatabase.Authors.Count + 1,
//                request.FirstName,
//                request.LastName
//            );

//            _fakeDatabase.Authors.Add(newAuthor);

//            return Task.FromResult(newAuthor);
//        }
//    }
//}
