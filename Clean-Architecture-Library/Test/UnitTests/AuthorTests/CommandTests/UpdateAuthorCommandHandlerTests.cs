using Application.Commands.Authors.UpdateAuthor;
using Application.DTOs.AuthorDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.UnitTests.AuthorTests.CommandTests
{
    public class UpdateAuthorCommandHandlerTests
    {
        private Mock<ICommandRepository<Author>> _mockCommandRepository;
        private Mock<IQueryRepository<Author>> _mockQueryRepository;
        private Mock<IValidator<UpdateAuthorDto>> _mockValidator;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<UpdateAuthorCommandHandler>> _mockLogger;
        private UpdateAuthorCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockCommandRepository = new Mock<ICommandRepository<Author>>();
            _mockQueryRepository = new Mock<IQueryRepository<Author>>();
            _mockValidator = new Mock<IValidator<UpdateAuthorDto>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UpdateAuthorCommandHandler>>();

            _handler = new UpdateAuthorCommandHandler(
                _mockCommandRepository.Object,
                _mockQueryRepository.Object,
                _mockValidator.Object,
                _mockMapper.Object,
                _mockLogger.Object
                );
        }

        [Test]
        public async Task Handle_AuthorExists_ReturnsSuccessOperationResult()
        {
            //Arrange
            var authorId = Guid.NewGuid();
            var updateAuthorDto = new UpdateAuthorDto { FirstName = "John", LastName = "Doe" };
            var command = new UpdateAuthorCommand(authorId, updateAuthorDto);

            var existingAuthor = new Author { Id = authorId };
            var updatedAuthorDto = new AuthorDto { Id = authorId, FirstName = "John", LastName = "Doe" };

            _mockValidator.Setup(validator => validator.ValidateAsync(updateAuthorDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _mockQueryRepository.Setup(repo => repo.GetByIdAsync(authorId))
                                .ReturnsAsync(existingAuthor);

            _mockMapper.Setup(mapper => mapper.Map<AuthorDto>(existingAuthor))
                       .Returns(updatedAuthorDto);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Author updated successfully."));
            _mockCommandRepository.Verify(repo => repo.UpdateAsync(existingAuthor), Times.Once);
        }

        [Test]
        public async Task Handle_AuthorDoesNotExist_ReturnsFailureOperationResult()
        {
            //Arrange
            var authorId = Guid.NewGuid();
            var updateAuthorDto = new UpdateAuthorDto { FirstName = "John", LastName = "Doe" };
            var command = new UpdateAuthorCommand(authorId, updateAuthorDto);

            _mockValidator.Setup(validator => validator.ValidateAsync(updateAuthorDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Error: Author not found."));
            _mockCommandRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Author>()), Times.Never);
        }

        [Test]
        public async Task Handle_InvalidDto_ReturnsFailureOperationResult()
        {
            //Arrange
            var authorId = Guid.NewGuid();
            var updateAuthorDto = new UpdateAuthorDto { FirstName = "", LastName = "Doe" };
            var command = new UpdateAuthorCommand(authorId, updateAuthorDto);

            var validationFailures = new[] { new ValidationFailure("FirstName", "First name is required.") };

            _mockValidator.Setup(validator => validator.ValidateAsync(updateAuthorDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(validationFailures));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Validation failed."));
            Assert.That(result.ErrorMessage, Is.EqualTo("First name is required."));
            _mockCommandRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Author>()), Times.Never);
        }
    }
}
