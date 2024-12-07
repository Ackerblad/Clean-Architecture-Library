using Application.DTOs.BookDtos;
using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Books.GetAllBooks;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.UnitTests.BookTests.QueryTests
{
    public class GetAllBooksQueryHandlerTests
    {
        private Mock<IQueryRepository<Book>> _mockBookRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<GetAllBooksQueryHandler>> _mockLogger;
        private GetAllBooksQueryHandler _handler;
        private Mock<IMemoryCache> _mockMemoryCache;

        [SetUp]
        public void SetUp()
        {
            _mockBookRepository = new Mock<IQueryRepository<Book>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetAllBooksQueryHandler>>();
            _mockMemoryCache = new Mock<IMemoryCache>();

            _handler = new GetAllBooksQueryHandler(
                _mockBookRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockMemoryCache.Object
                );
        }

        [Test]
        public async Task Handle_NoCache_ReturnsAllBooks()
        {
            //Arrange
            var books = new List<Book> { new Book(), new Book() };
            var bookDtos = new List<BookDto> { new BookDto(), new BookDto() };
            object cachedData = null!;
            var query = new GetAllBooksQuery();

            _mockMemoryCache.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out cachedData!))
                           .Returns(false);

            _mockMemoryCache.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                            .Returns(Mock.Of<ICacheEntry>);

            _mockBookRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(books);

            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<BookDto>>(books))
                       .Returns(bookDtos);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.That(result, Is.EqualTo(bookDtos));
            _mockBookRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<IEnumerable<BookDto>>(books), Times.Once);
            _mockMemoryCache.Verify(cache => cache.CreateEntry(It.IsAny<object>()), Times.Once);

        }

        [Test]
        public async Task Handle_CacheExists_ReturnsCachedBooks()
        {
            //Arrange
            var books = new List<Book> { new Book(), new Book() };
            var bookDtos = new List<BookDto> { new BookDto(), new BookDto() };
            object cachedData = books;
            var query = new GetAllBooksQuery();

            _mockMemoryCache.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out cachedData!))
                            .Returns(true);

            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<BookDto>>(books))
                       .Returns(bookDtos);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.That(result, Is.EqualTo(bookDtos));
            _mockBookRepository.Verify(repo => repo.GetAllAsync(), Times.Never);
            _mockMapper.Verify(mapper => mapper.Map<IEnumerable<BookDto>>(books), Times.Once);
            _mockMemoryCache.Verify(cache => cache.TryGetValue(It.IsAny<object>(), out cachedData!), Times.Once);
        }
    }
}
