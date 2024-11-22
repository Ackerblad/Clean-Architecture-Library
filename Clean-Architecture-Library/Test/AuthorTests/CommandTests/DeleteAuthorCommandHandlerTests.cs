using Application.Commands.Authors.DeleteAuthor;
using Infrastructure;
using System;

namespace Test.AuthorTests.CommandTests
{
    public class DeleteAuthorCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private DeleteAuthorCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            _handler = new DeleteAuthorCommandHandler(_fakeDatabase);
        }

        [Test]
        public async Task Handle_AuthorExistsAndNoBooksAssociated_DeletesAuthorFromDatabase()
        {
            //Arrange
            var authorToDelete = _fakeDatabase.Authors.First(a => !_fakeDatabase.Books.Any(b => b.AuthorId == a.Id));
            var initialAuthors = _fakeDatabase.Authors.Count;
            var command = new DeleteAuthorCommand(authorToDelete.Id);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.That(result, Is.True);
            Assert.That(_fakeDatabase.Authors.Count, Is.EqualTo(initialAuthors - 1));
        }

        [Test]
        public void Handle_AuthorDoesNotExist_ThrowsKeyNotFoundException()
        {
            //Arrange
            var initialAuthors = _fakeDatabase.Authors.Count;
            var command = new DeleteAuthorCommand(99);

            //Act
            Task action() => _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.ThrowsAsync<KeyNotFoundException>(action);
            Assert.That(_fakeDatabase.Authors.Count, Is.EqualTo(initialAuthors));
        }

        [Test]
        public void Handle_AuthorAssociatedWithBooks_ThrowsInvalidOperationException()
        {
            //Arrange
            var authorToDelete = _fakeDatabase.Authors.First(a => _fakeDatabase.Books.Any(b => b.AuthorId == a.Id));
            var initialAuthors = _fakeDatabase.Authors.Count;
            var command = new DeleteAuthorCommand(authorToDelete.Id);

            //Act
            Task action() => _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(action);
            Assert.That(_fakeDatabase.Authors.Count, Is.EqualTo(initialAuthors));
        }
    }
}
