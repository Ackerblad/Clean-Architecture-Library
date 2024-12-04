//using Application.Queries.Authors.GetAllAuthors;
//using Infrastructure;

//namespace Test.AuthorTests.QueryTests
//{
//    public class GetAllAuthorsQueryHandlerTests
//    {
//        private FakeDatabase _fakeDatabase;
//        private GetAllAuthorsQueryHandler _handler;

//        [SetUp]
//        public void SetUp()
//        {
//            _fakeDatabase = new FakeDatabase();
//            _handler = new GetAllAuthorsQueryHandler(_fakeDatabase);
//        }

//        [Test]
//        public async Task Handle_ReturnsAllAuthors()
//        {
//            //Arrange
//            var query = new GetAllAuthorsQuery();

//            //Act
//            var result = await _handler.Handle(query, CancellationToken.None);

//            //Assert
//            Assert.That(result, Is.Not.Null);
//            Assert.That(result.Count, Is.EqualTo(_fakeDatabase.Authors.Count));
//        }
//    }
//}
