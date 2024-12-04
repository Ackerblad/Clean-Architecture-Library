//using Domain.Entities;
//using Infrastructure;
//using MediatR;

//namespace Application.Queries.Books.GetAllBooks
//{
//    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<Book>>
//    {
//        private readonly FakeDatabase _fakeDatabase;

//        public GetAllBooksQueryHandler(FakeDatabase fakeDatabase)
//        {
//            _fakeDatabase = fakeDatabase;
//        }

//        public Task<List<Book>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(_fakeDatabase.Books);
//        }
//    }
//}
