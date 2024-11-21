using Application.Queries.Books.GetBookById;
using Infrastructure;

namespace Test.BookTests.QueryTests
{
    public class GetBookByIdQueryHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private GetBookByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fakeDatabase = new FakeDatabase();
            _handler = new GetBookByIdQueryHandler(_fakeDatabase);
        }

        [Test]
        public async Task Handle_BookExists_ReturnsBook()
        {
            //Arrange
            var existingBook = _fakeDatabase.Books[0];
            var query = new GetBookByIdQuery(existingBook.Id);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(existingBook.Id));
            Assert.That(result.Title, Is.EqualTo(existingBook.Title));
            Assert.That(result.AuthorId, Is.EqualTo(existingBook.AuthorId));
        }

        [Test]
        public void Handle_BookDoesNotExist_ThrowsKeyNotFoundException()
        {
            //Arrange
            var query = new GetBookByIdQuery(99);

            //Act
            Task action() => _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(action);
        }
    }
}
