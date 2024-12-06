using Application.DTOs.AuthorDtos;
using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Authors.GetAllAuthors;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
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
        private Mock<IMemoryCache> _mockMemoryCache;

        [SetUp]
        public void SetUp()
        {
            _mockAuthorRepository = new Mock<IQueryRepository<Author>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetAllAuthorsQueryHandler>>();
            _mockMemoryCache = new Mock<IMemoryCache>();

            _handler = new GetAllAuthorsQueryHandler(
                _mockAuthorRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockMemoryCache.Object
                );
        }

        [Test]
        public async Task Handle_NoCache_ReturnsAllAuthors()
        {
            //Arrange
            var authors = new List<Author> { new Author(), new Author() };
            var authorDtos = new List<AuthorDto> { new AuthorDto(), new AuthorDto() };
            object cachedData = null;
            var query = new GetAllAuthorsQuery();

            _mockMemoryCache.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                           .Returns(false);

            _mockMemoryCache.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                            .Returns(Mock.Of<ICacheEntry>);

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
            _mockMemoryCache.Verify(cache => cache.CreateEntry(It.IsAny<object>()), Times.Once);
        }

        [Test]
        public async Task Handle_CacheExists_ReturnsCachedAuthors()
        {
            //Arrange
            var authors = new List<Author> { new Author(), new Author() };
            var authorDtos = new List<AuthorDto> { new AuthorDto(), new AuthorDto() };
            object cachedData = authors;
            var query = new GetAllAuthorsQuery();

            _mockMemoryCache
                .Setup(cache => cache.TryGetValue(It.IsAny<object>(), out cachedData))
                .Returns(true);

            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<AuthorDto>>(authors))
                       .Returns(authorDtos);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.That(result, Is.EqualTo(authorDtos));
            _mockAuthorRepository.Verify(repo => repo.GetAllAsync(), Times.Never);
            _mockMapper.Verify(mapper => mapper.Map<IEnumerable<AuthorDto>>(authors), Times.Once);
            _mockMemoryCache.Verify(cache => cache.TryGetValue(It.IsAny<object>(), out cachedData), Times.Once);
        }
    }
}
