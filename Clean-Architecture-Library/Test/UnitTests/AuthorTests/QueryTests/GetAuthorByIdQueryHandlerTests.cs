using Application.DTOs.AuthorDtos;
using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Authors.GetAuthorById;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.UnitTests.AuthorTests.QueryTests
{
    public class GetAuthorByIdQueryHandlerTests
    {
        private Mock<IQueryRepository<Author>> _mockAuthorRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<GetAuthorByIdQueryHandler>> _mockLogger;
        private GetAuthorByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockAuthorRepository = new Mock<IQueryRepository<Author>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetAuthorByIdQueryHandler>>();

            _handler = new GetAuthorByIdQueryHandler(
                _mockAuthorRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object
                );
        }

        [Test]
        public async Task Handle_AuthorExists_ReturnsSuccessOperationResult()
        {
            //Arrange
            var authorId = Guid.NewGuid();
            var author = new Author { Id = authorId };
            var authorDto = new AuthorDto { Id = authorId };
            var query = new GetAuthorByIdQuery(authorId);

            _mockAuthorRepository.Setup(repo => repo.GetByIdAsync(authorId))
                                 .ReturnsAsync(author);

            _mockMapper.Setup(mapper => mapper.Map<AuthorDto>(author))
                       .Returns(authorDto);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Author retrieved successfully."));
            Assert.That(result.Data, Is.EqualTo(authorDto));
            _mockAuthorRepository.Verify(repo => repo.GetByIdAsync(authorId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<AuthorDto>(author), Times.Once);
        }

        [Test]
        public async Task Handle_AuthorDoesNotExist_ReturnsFailureOperationResult()
        {
            //Arrange
            var authorId = Guid.NewGuid();
            var query = new GetAuthorByIdQuery(authorId);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Error: Author Not Found"));
            _mockAuthorRepository.Verify(repo => repo.GetByIdAsync(authorId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<AuthorDto>(It.IsAny<Author>()), Times.Never);
        }
    }
}
