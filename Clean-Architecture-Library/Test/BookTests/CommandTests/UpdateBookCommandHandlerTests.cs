using Application.Commands.Books.UpdateBook;
using Application.DTOs.BookDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.BookTests.CommandTests
{
    public class UpdateBookCommandHandlerTests
    {
        private Mock<ICommandRepository<Book>> _mockCommandRepository;
        private Mock<IQueryRepository<Book>> _mockQueryRepository;
        private Mock<IQueryRepository<Author>> _mockAuthorRepository;
        private Mock<IValidator<UpdateBookDto>> _mockValidator;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<UpdateBookCommandHandler>> _mockLogger;
        private UpdateBookCommandHandler _handler;


        [SetUp]
        public void SetUp()
        {
            _mockCommandRepository = new Mock<ICommandRepository<Book>>();
            _mockQueryRepository = new Mock<IQueryRepository<Book>>();
            _mockAuthorRepository = new Mock<IQueryRepository<Author>>();
            _mockValidator = new Mock<IValidator<UpdateBookDto>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UpdateBookCommandHandler>>();

            _handler = new UpdateBookCommandHandler(
                _mockCommandRepository.Object,
                _mockQueryRepository.Object,
                _mockAuthorRepository.Object,
                _mockValidator.Object,
                _mockMapper.Object,
                _mockLogger.Object
                );
        }

        [Test]
        public async Task Handle_BookExists_ReturnsSuccessOperationResult()
        {
            //Arrange
            var bookId = Guid.NewGuid();
            var updateBookDto = new UpdateBookDto { Title = "Updated Title", Description = "Updated Description", AuthorId = Guid.NewGuid() };
            var command = new UpdateBookCommand(bookId, updateBookDto);

            var existingBook = new Book { Id = bookId };
            var author = new Author { Id = updateBookDto.AuthorId };
            var updatedBookDto = new BookDto { Id = bookId, Title = "Updated Title", Description = "Updated Description", AuthorId = updateBookDto.AuthorId };

            _mockValidator.Setup(validator => validator.ValidateAsync(updateBookDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _mockQueryRepository.Setup(repo => repo.GetByIdAsync(bookId))
                                .ReturnsAsync(existingBook);

            _mockAuthorRepository.Setup(repo => repo.GetByIdAsync(updateBookDto.AuthorId))
                                 .ReturnsAsync(author);

            _mockMapper.Setup(mapper => mapper.Map(updateBookDto, existingBook))
                        .Returns(existingBook);

            _mockMapper.Setup(mapper => mapper.Map<BookDto>(existingBook))
                       .Returns(updatedBookDto);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Book updated successfully."));
            _mockCommandRepository.Verify(repo => repo.UpdateAsync(existingBook), Times.Once);
        }

        [Test]
        public async Task Handle_BookDoesNotExist_ReturnsFailureOperationResult()
        {
            //Arrange
            var bookId = Guid.NewGuid();
            var updateBookDto = new UpdateBookDto { Title = "Updated Title", Description = "Updated Description", AuthorId = Guid.NewGuid() };
            var command = new UpdateBookCommand(bookId, updateBookDto);

            _mockValidator.Setup(validator => validator.ValidateAsync(updateBookDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Error: Book not found."));
            _mockCommandRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }

        [Test]
        public async Task Handle_AuthorDoesNotExist_ReturnsFailureOperationResult()
        {
            //Arrange
            var bookId = Guid.NewGuid();
            var updateBookDto = new UpdateBookDto { Title = "Updated Title", Description = "Updated Description", AuthorId = Guid.NewGuid() };
            var command = new UpdateBookCommand(bookId, updateBookDto);
            var existingBook = new Book { Id = bookId };

            _mockQueryRepository.Setup(repo => repo.GetByIdAsync(bookId))
                                .ReturnsAsync(existingBook);

            _mockValidator.Setup(validator => validator.ValidateAsync(updateBookDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Error: Author not found."));
            _mockCommandRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }

        [Test]
        public async Task Handle_InvalidDto_ReturnsFailureOperationResult()
        {
            //Arrange
            var bookId = Guid.NewGuid();
            var updateBookDto = new UpdateBookDto { Title = "", Description = "Updated Description", AuthorId = Guid.NewGuid() };
            var command = new UpdateBookCommand(bookId, updateBookDto);

            var validationErrors = new[] { new ValidationFailure("Title", "Title is required.") };

            _mockValidator.Setup(validator => validator.ValidateAsync(updateBookDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(validationErrors));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Validation failed."));
            Assert.That(result.ErrorMessage, Is.EqualTo("Title is required."));
            _mockCommandRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }
    }
}
