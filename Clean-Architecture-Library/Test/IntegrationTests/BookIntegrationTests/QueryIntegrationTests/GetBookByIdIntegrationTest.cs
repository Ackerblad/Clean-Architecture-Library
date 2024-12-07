using Application.Queries.Books.GetBookById;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.IntegrationTests.BookIntegrationTests.QueryIntegrationTests
{
    public class GetBookByIdIntegrationTest
    {
        private CleanArchitectureLibraryDb _dbContext;
        private QueryRepository<Book> _bookRepository;
        private IMapper _mapper;
        private Mock<ILogger<GetBookByIdQueryHandler>> _mockLogger;
        private GetBookByIdQueryHandler _handler;

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

            _mockLogger = new Mock<ILogger<GetBookByIdQueryHandler>>();

            _handler = new GetBookByIdQueryHandler(
                _bookRepository,
                _mapper,
                _mockLogger.Object
            );
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task Handle_BookExists_ReturnsCorrectBook()
        {
            //Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Test Book",
                Description = "Test Description"
            };
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();

            var query = new GetBookByIdQuery(book.Id);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Data.Title, Is.EqualTo("Test Book"));
            Assert.That(result.Data.Description, Is.EqualTo("Test Description"));
        }

        [Test]
        public async Task Handle_BookDoesNotExist_ReturnsFailure()
        {
            //Arrange
            var query = new GetBookByIdQuery(Guid.NewGuid()); 

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Error: Book Not Found"));
        }
    }
}
