using Application.Commands.Authors.UpdateAuthor;
using Infrastructure;

namespace Test.AuthorTests.CommandTests
{
    public class UpdateAuthorCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private UpdateAuthorCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            _handler = new UpdateAuthorCommandHandler(_fakeDatabase);
        }

        [Test]
        public async Task Handle_AuthorExists_UpdatesAuthor()
        {
            //Arrange
            var existingAuthor = _fakeDatabase.Authors[0];
            var command = new UpdateAuthorCommand(existingAuthor.Id, "Updated", "Author");

            //Act
            var updatedAuthor = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.That(updatedAuthor.FirstName, Is.EqualTo("Updated"));
            Assert.That(updatedAuthor.LastName, Is.EqualTo("Author"));
        }

        [Test]
        public void Handle_AuthorDoesNotExist_ThrowsKeyNotFoundException()
        {
            //Arrange
            var command = new UpdateAuthorCommand(99, "Updated", "Author");

            //Act
            Task action() => _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(action);
            Assert.That(_fakeDatabase.Authors.Any(a => a.Id == 99), Is.False);
        }
    }
}
