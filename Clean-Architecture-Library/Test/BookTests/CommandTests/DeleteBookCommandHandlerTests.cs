using Application.Commands.Books.DeleteBook;
using Infrastructure;

namespace Test.BookTests.CommandTests
{
    public class DeleteBookCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private DeleteBookCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fakeDatabase = new FakeDatabase();
            _handler = new DeleteBookCommandHandler(_fakeDatabase);
        }

        [Test]
        public async Task Handle_BookExists_DeletesBookFromDatabase()
        {
            // Arrange
            var bookToDelete = _fakeDatabase.Books[0];
            var initialBooks = _fakeDatabase.Books.Count;
            var command = new DeleteBookCommand(bookToDelete.Id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.True); 
            Assert.That(_fakeDatabase.Books.Count, Is.EqualTo(initialBooks - 1)); 
        }

        [Test]
        public void Handle_BookDoesNotExist_ThrowsKeyNotFoundException()
        {
            //Arrange
            var initialBooks = _fakeDatabase.Books.Count;
            var command = new DeleteBookCommand(99);

            //Act
            Task action() => _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(action);
            Assert.That(_fakeDatabase.Books.Count, Is.EqualTo(initialBooks));
        }
    }
}
