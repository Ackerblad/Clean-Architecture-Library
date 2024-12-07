using Application.Commands.Books.CreateBook;
using Application.DTOs.BookDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.UnitTests.BookTests.CommandTests
{
    public class CreateBookCommandHandlerTests
    {
        private Mock<ICommandRepository<Book>> _mockBookRepository;
        private Mock<IQueryRepository<Author>> _mockAuthorRepository;
        private Mock<IValidator<CreateBookDto>> _mockValidator;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<CreateBookCommandHandler>> _mockLogger;
        private CreateBookCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockBookRepository = new Mock<ICommandRepository<Book>>();
            _mockAuthorRepository = new Mock<IQueryRepository<Author>>();
            _mockValidator = new Mock<IValidator<CreateBookDto>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CreateBookCommandHandler>>();

            _handler = new CreateBookCommandHandler(
                _mockBookRepository.Object,
                _mockAuthorRepository.Object,
                _mockValidator.Object,
                _mockMapper.Object,
                _mockLogger.Object
                );
        }

        [Test]
        public async Task Handle_AuthorExists_ReturnsSuccessOperationResult()
        {
            //Arrange
            var newBookDto = new CreateBookDto { Title = "Test Book", AuthorId = Guid.NewGuid() };
            var command = new CreateBookCommand(newBookDto);
            var bookEntity = new Book();
            var bookDto = new BookDto { Title = "Test Book" };

            _mockValidator.Setup(validator => validator.ValidateAsync(newBookDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _mockAuthorRepository.Setup(repo => repo.GetByIdAsync(newBookDto.AuthorId))
                                 .ReturnsAsync(new Author());

            _mockMapper.Setup(mapper => mapper.Map<Book>(newBookDto))
                       .Returns(bookEntity);

            _mockMapper.Setup(mapper => mapper.Map<BookDto>(bookEntity))
                       .Returns(bookDto);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Book created successfully."));
            _mockBookRepository.Verify(repo => repo.CreateAsync(bookEntity), Times.Once);
        }

        [Test]
        public async Task Handle_AuthorDoesNotExist_ReturnsFailureOperationResult()
        {
            //Arrange
            var newBookDto = new CreateBookDto { Title = "Test Book", AuthorId = Guid.NewGuid() };
            var command = new CreateBookCommand(newBookDto);

            _mockValidator.Setup(validator => validator.ValidateAsync(newBookDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Error: Author not found."));
            _mockBookRepository.Verify(repo => repo.CreateAsync(It.IsAny<Book>()), Times.Never);
        }

        [Test]
        public async Task Handle_InvalidDto_ReturnsFailureOperationResult()
        {
            //Arrange
            var newBookDto = new CreateBookDto { Title = "", AuthorId = Guid.NewGuid() };
            var command = new CreateBookCommand(newBookDto);
            var validationErrors = new[] { new ValidationFailure("Title", "Title is required.") };

            _mockValidator.Setup(validator => validator.ValidateAsync(newBookDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(validationErrors));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Validation failed."));
            Assert.That(result.ErrorMessage, Is.EqualTo("Title is required."));
            _mockBookRepository.Verify(repo => repo.CreateAsync(It.IsAny<Book>()), Times.Never);
        }
    }
}
