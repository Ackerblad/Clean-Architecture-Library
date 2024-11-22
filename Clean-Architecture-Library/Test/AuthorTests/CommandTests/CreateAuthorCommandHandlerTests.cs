using Application.Commands.Authors.CreateAuthor;
using Infrastructure;

namespace Test.AuthorTests.CommandTests
{
    public class CreateAuthorCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private CreateAuthorCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fakeDatabase = new FakeDatabase();
            _handler = new CreateAuthorCommandHandler(_fakeDatabase);
        }

        [Test]
        public async Task Handle_AuthorIsUnique_AddsAuthorToDatabase()
        {
            //Arrange
            var initialAuthors = _fakeDatabase.Authors.Count;
            var command = new CreateAuthorCommand("New", "Author");

            //Act
            var createdAuthor = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.That(createdAuthor.FirstName, Is.EqualTo("New"));
            Assert.That(createdAuthor.LastName, Is.EqualTo("Author"));
            Assert.That(_fakeDatabase.Authors.Count, Is.EqualTo(initialAuthors + 1));
        }

        [Test]
        public void Handle_AuthorAlreadyExists_ThrowsInvalidOperationException()
        {
            //Arrange
            var initialAuthors = _fakeDatabase.Authors.Count;
            var existingAuthor = _fakeDatabase.Authors[0];
            var command = new CreateAuthorCommand(existingAuthor.FirstName, existingAuthor.LastName);

            //Act
            Task action() => _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(action);
            Assert.That(_fakeDatabase.Authors.Count, Is.EqualTo(initialAuthors));
        }
    }
}
