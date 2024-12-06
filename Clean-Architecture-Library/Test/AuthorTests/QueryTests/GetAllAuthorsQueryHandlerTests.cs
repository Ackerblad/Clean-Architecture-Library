using Application.DTOs.AuthorDtos;
using Application.DTOs.BookDtos;
using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Authors.GetAllAuthors;
using Application.Queries.Books.GetAllBooks;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.AuthorTests.QueryTests
{
    public class GetAllAuthorsQueryHandlerTests
    {
        private Mock<IQueryRepository<Author>> _mockAuthorRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<GetAllAuthorsQueryHandler>> _mockLogger;
        private GetAllAuthorsQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockAuthorRepository = new Mock<IQueryRepository<Author>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetAllAuthorsQueryHandler>>();

            _handler = new GetAllAuthorsQueryHandler(
                _mockAuthorRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object
                );
        }

        [Test]
        public async Task Handle_ReturnsAllAuthors()
        {
            //Arrange
            var authors = new List<Author> { new Author(), new Author() };
            var authorDtos = new List<AuthorDto> { new AuthorDto(), new AuthorDto() };
            var query = new GetAllAuthorsQuery();

            _mockAuthorRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(authors);

            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<AuthorDto>>(authors))
                       .Returns(authorDtos);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.That(result, Is.EqualTo(authorDtos));
            _mockAuthorRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<IEnumerable<AuthorDto>>(authors), Times.Once);
        }
    }
}
