using Application.Commands.Authors.CreateAuthor;
using Application.DTOs.AuthorDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.AuthorTests.CommandTests
{
    public class CreateAuthorCommandHandlerTests
    {
        private Mock<ICommandRepository<Author>> _mockAuthorRepository;
        private Mock<IValidator<CreateAuthorDto>> _mockValidator;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<CreateAuthorCommandHandler>> _mockLogger;
        private CreateAuthorCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockAuthorRepository = new Mock<ICommandRepository<Author>>();
            _mockValidator = new Mock<IValidator<CreateAuthorDto>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CreateAuthorCommandHandler>>();

            _handler = new CreateAuthorCommandHandler(
                _mockAuthorRepository.Object,
                _mockValidator.Object,
                _mockMapper.Object,
                _mockLogger.Object
                );
        }

        [Test]
        public async Task Handle_ValidDto_ReturnsSuccessOperationResult()
        {
            //Arrange
            var newAuthorDto = new CreateAuthorDto { FirstName = "John", LastName = "Doe" };
            var command = new CreateAuthorCommand(newAuthorDto);
            var authorEntity = new Author();
            var authorDto = new AuthorDto { Id = authorEntity.Id, FirstName = "John", LastName = "Doe" };

            _mockValidator.Setup(validator => validator.ValidateAsync(newAuthorDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _mockMapper.Setup(mapper => mapper.Map<Author>(newAuthorDto))
                       .Returns(authorEntity);

            _mockMapper.Setup(mapper => mapper.Map<AuthorDto>(authorEntity))
                       .Returns(authorDto);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Author created successfully."));
            _mockAuthorRepository.Verify(repo => repo.CreateAsync(authorEntity), Times.Once);
        }

        [Test]
        public async Task Handle_InvalidDto_ReturnsFailureOperationResult()
        {
            //Arrange
            var newAuthorDto = new CreateAuthorDto { FirstName = "", LastName = "Doe" };
            var command = new CreateAuthorCommand(newAuthorDto);
            var validationFailures = new[] { new ValidationFailure("FirstName", "First name is required.") };

            _mockValidator.Setup(validator => validator.ValidateAsync(newAuthorDto, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(validationFailures));

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Validation failed."));
            Assert.That(result.ErrorMessage, Is.EqualTo("First name is required."));
            _mockAuthorRepository.Verify(repo => repo.CreateAsync(It.IsAny<Author>()), Times.Never);
        }
    }
}
