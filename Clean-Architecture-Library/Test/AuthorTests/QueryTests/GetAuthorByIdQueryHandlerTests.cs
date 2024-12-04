//using Application.Queries.Authors.GetAuthorById;
//using Infrastructure;

//namespace Test.AuthorTests.QueryTests
//{
//    public class GetAuthorByIdQueryHandlerTests
//    {
//        private FakeDatabase _fakeDatabase;
//        private GetAuthorByIdQueryHandler _handler;

//        [SetUp]
//        public void SetUp()
//        {
//            _fakeDatabase = new FakeDatabase();
//            _handler = new GetAuthorByIdQueryHandler(_fakeDatabase);
//        }

//        [Test]
//        public async Task Handle_AuthorExists_ReturnsAuthor()
//        {
//            //Arrange
//            var existingAuthor = _fakeDatabase.Authors[0];
//            var query = new GetAuthorByIdQuery(existingAuthor.Id);

//            //Act
//            var result = await _handler.Handle(query, CancellationToken.None);

//            //Assert
//            Assert.That(result, Is.Not.Null);
//            Assert.That(result.Id, Is.EqualTo(existingAuthor.Id));
//            Assert.That(result.LastName, Is.EqualTo(existingAuthor.LastName));

//        }

//        [Test]
//        public void Handle_AuthorDoesNotExist_ThrowsKeyNotFoundException()
//        {
//            //Arrange
//            var query = new GetAuthorByIdQuery(99);

//            //Act
//            Task action() => _handler.Handle(query, CancellationToken.None);

//            //Assert
//            Assert.ThrowsAsync<KeyNotFoundException>(action);
//        }
//    }
//}
