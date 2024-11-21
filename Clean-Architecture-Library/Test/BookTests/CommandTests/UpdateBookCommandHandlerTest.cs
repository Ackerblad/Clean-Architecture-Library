using Application.Commands.Books.UpdateBook;
using Infrastructure;

namespace Test.BookTests.CommandTests
{
    public class UpdateBookCommandHandlerTest
    {
        private FakeDatabase _fakeDatabase;
        private UpdateBookCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fakeDatabase = new FakeDatabase();
            _handler = new UpdateBookCommandHandler(_fakeDatabase);
        }

        [Test]
        public async Task Handle_BookExists_UpdatesBook()
        {
            //Arrange
            var existingBook = _fakeDatabase.Books[0];
            var command = new UpdateBookCommand(existingBook.Id, "UpdatedTitle", "UpdatedDescription", 1);

            //Act
            var updatedBook = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.That(updatedBook.Title, Is.EqualTo("UpdatedTitle"));
            Assert.That(updatedBook.Description, Is.EqualTo("UpdatedDescription"));
            Assert.That(updatedBook.AuthorId, Is.EqualTo(1));
        }

        [Test]
        public async Task Handle_BookDoesNotExist_ThrowsKeyNotFoundException()
        {
            //Arrange
            var command = new UpdateBookCommand(99, "UpdatedTitle", "UpdatedDescription", 1);

            //Act
            Task action() => _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(action);
            Assert.That(_fakeDatabase.Books.Any(b => b.Id == 99), Is.False);
        }

        [Test]
        public void Handle_InvalidAuthorId_ThrowsKeyNotFoundException()
        {
            //Arrange
            var existingBook = _fakeDatabase.Books[0];
            var command = new UpdateBookCommand(existingBook.Id, "UpdatedTitle", "UpdatedDescription", 99);

            //Act
            Task action() => _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(action);
            Assert.That(_fakeDatabase.Books.Any(b => b.AuthorId == 99), Is.False);
        }
    }
}
