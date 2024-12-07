using Application.Commands.Books.DeleteBook;
using Domain.Entities;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.IntegrationTests.BookIntegrationTests.CommandIntegrationTests
{
    public class DeleteBookIntegrationTest
    {
        private CleanArchitectureLibraryDb _dbContext;
        private CommandRepository<Book> _commandRepository;
        private QueryRepository<Book> _queryRepository;
        private Mock<ILogger<DeleteBookCommandHandler>> _mockLogger;
        private DeleteBookCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            //Configure InMemoryDatabase
            var options = new DbContextOptionsBuilder<CleanArchitectureLibraryDb>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _dbContext = new CleanArchitectureLibraryDb(options);

            //Initialize repositories with InMemoryDatabase
            _commandRepository = new CommandRepository<Book>(_dbContext);
            _queryRepository = new QueryRepository<Book>(_dbContext);

            _mockLogger = new Mock<ILogger<DeleteBookCommandHandler>>();

            _handler = new DeleteBookCommandHandler(
                _commandRepository,
                _queryRepository,
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
        public async Task Handle_ValidInput_DeletesBookSuccessfully()
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

            var command = new DeleteBookCommand(book.Id);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Book deleted successfully."));

            var deletedBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            Assert.IsNull(deletedBook);
        }

        [Test]
        public async Task Handle_BookDoesNotExist_ReturnsFailure()
        {
            //Arrange
            var command = new DeleteBookCommand(Guid.NewGuid());

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Error: Book not found."));
        }
    }
}
