using Application.Commands.Books.DeleteBook;
using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.BookTests.CommandTests
{
    public class DeleteBookCommandHandlerTests
    {
        private Mock<ICommandRepository<Book>> _mockCommandRepository;
        private Mock<IQueryRepository<Book>> _mockQueryRepository;
        private Mock<ILogger<DeleteBookCommandHandler>> _mockLogger;
        private DeleteBookCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockCommandRepository = new Mock<ICommandRepository<Book>>();
            _mockQueryRepository = new Mock<IQueryRepository<Book>>();
            _mockLogger = new Mock<ILogger<DeleteBookCommandHandler>>();

            _handler = new DeleteBookCommandHandler(
                _mockCommandRepository.Object,
                _mockQueryRepository.Object,
                _mockLogger.Object
            );
        }

        [Test]
        public async Task Handle_BookExists_ReturnsSuccessOperationResult()
        {
            //Arrange
            var bookId = Guid.NewGuid();
            var command = new DeleteBookCommand(bookId);

            _mockQueryRepository.Setup(repo => repo.GetByIdAsync(bookId))
                                .ReturnsAsync(new Book { Id = bookId });

            _mockCommandRepository.Setup(repo => repo.DeleteAsync(bookId))
                                  .ReturnsAsync(true);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Book deleted successfully."));
            _mockCommandRepository.Verify(repo => repo.DeleteAsync(bookId), Times.Once);
        }

        [Test]
        public async Task Handle_BookDoesNotExist_ReturnsFailureOperationResult()
        {
            //Arrange
            var bookId = Guid.NewGuid();
            var command = new DeleteBookCommand(bookId);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Error: Book not found."));
            _mockCommandRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
