using Application.Commands.Authors.DeleteAuthor;
using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.AuthorTests.CommandTests
{
    public class DeleteAuthorCommandHandlerTests
    {
        private Mock<ICommandRepository<Author>> _mockCommandRepository;
        private Mock<IQueryRepository<Author>> _mockQueryRepository;
        private Mock<IQueryRepository<Book>> _mockBookRepository;
        private Mock<ILogger<DeleteAuthorCommandHandler>> _mockLogger;
        private DeleteAuthorCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockCommandRepository = new Mock<ICommandRepository<Author>>();
            _mockQueryRepository = new Mock<IQueryRepository<Author>>();
            _mockBookRepository = new Mock<IQueryRepository<Book>>();
            _mockLogger = new Mock<ILogger<DeleteAuthorCommandHandler>>();

            _handler = new DeleteAuthorCommandHandler(
                _mockCommandRepository.Object,
                _mockQueryRepository.Object,
                _mockBookRepository.Object,
                _mockLogger.Object
                );
        }

        [Test]
        public async Task Handle_AuthorExists_ReturnsSuccessOperationResult()
        {
            //Arrange
            var authorId = Guid.NewGuid();
            var command = new DeleteAuthorCommand(authorId);

            _mockQueryRepository.Setup(repo => repo.GetByIdAsync(authorId))
                                .ReturnsAsync(new Author { Id = authorId });

            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                                     .ReturnsAsync(new List<Book>());

            _mockCommandRepository.Setup(repo => repo.DeleteAsync(authorId))
                                   .ReturnsAsync(true);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Author deleted successfully."));
            _mockCommandRepository.Verify(repo => repo.DeleteAsync(authorId), Times.Once);
        }

        [Test]
        public async Task Handle_AuthorDoesNotExist_ReturnsFailureOperationResult()
        {
            //Arrange
            var authorId = Guid.NewGuid();
            var command = new DeleteAuthorCommand(authorId);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Error: Author not found."));
            _mockCommandRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public async Task Handle_AuthorAssociatedWithBooks_ReturnsFailureOperationResult()
        {
            //Arrange
            var authorId = Guid.NewGuid();
            var associatedBooks = new List<Book> { new Book { AuthorId = authorId } };
            var command = new DeleteAuthorCommand(authorId);

            _mockQueryRepository.Setup(repo => repo.GetByIdAsync(authorId))
                                .ReturnsAsync(new Author { Id = authorId });

            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                                     .ReturnsAsync(associatedBooks);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Error: Author has associated books."));
            _mockCommandRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
