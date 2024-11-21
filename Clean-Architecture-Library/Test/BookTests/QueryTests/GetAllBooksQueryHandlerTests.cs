using Application.Queries.Books.GetAllBooks;
using Infrastructure;

namespace Test.BookTests.QueryTests
{
    public class GetAllBooksQueryHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private GetAllBooksQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fakeDatabase = new FakeDatabase();
            _handler = new GetAllBooksQueryHandler(_fakeDatabase);
        }

        [Test]
        public async Task Handle_ReturnsAllBooks()
        {
            //Arrange
            var query = new GetAllBooksQuery();

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(_fakeDatabase.Books.Count));
        }
    }
}
