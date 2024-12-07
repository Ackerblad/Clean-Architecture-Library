using Application.Queries.Books.GetAllBooks;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.IntegrationTests.BookIntegrationTests.QueryIntegrationTests
{
    public class GetAllBookIntegrationTest
    {
        private CleanArchitectureLibraryDb _dbContext;
        private QueryRepository<Book> _bookRepository;
        private IMapper _mapper;
        private Mock<ILogger<GetAllBooksQueryHandler>> _mockLogger;
        private IMemoryCache _memoryCache;
        private GetAllBooksQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            //Configure InMemoryDatabase
            var options = new DbContextOptionsBuilder<CleanArchitectureLibraryDb>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _dbContext = new CleanArchitectureLibraryDb(options);

            //Initialize repository with InMemoryDatabase
            _bookRepository = new QueryRepository<Book>(_dbContext);

            //Set up AutoMapper
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<Application.MappingProfiles.BookProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _mockLogger = new Mock<ILogger<GetAllBooksQueryHandler>>();

            _memoryCache = new MemoryCache(new MemoryCacheOptions());

            _handler = new GetAllBooksQueryHandler(
                _bookRepository,
                _mapper,
                _mockLogger.Object,
                _memoryCache
            );
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task Handle_NoCache_ReturnsAllBooksAndCachesData()
        {
            //Arrange
            var book = new Book { Id = Guid.NewGuid(), Title = "Test Book", Description = "Test Description" };
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();

            var query = new GetAllBooksQuery();

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            var retrievedBook = result.FirstOrDefault();
            Assert.IsNotNull(retrievedBook);
            Assert.That(retrievedBook.Title, Is.EqualTo("Test Book"));

            //Verify cache
            Assert.IsTrue(_memoryCache.TryGetValue("allBooksCacheKey", out _));
        }

        [Test]
        public async Task Handle_CacheExists_ReturnsBooksFromCache()
        {
            //Arrange
            var cachedBooks = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Test Book", Description = "Test Description" }
            };

            _memoryCache.Set("allBooksCacheKey", cachedBooks, TimeSpan.FromMinutes(5));

            var query = new GetAllBooksQuery();

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            var retrievedBook = result.FirstOrDefault();
            Assert.IsNotNull(retrievedBook);
            Assert.That(retrievedBook.Title, Is.EqualTo("Test Book"));
        }
    }
}
