using Application.Commands.Books.UpdateBook;
using Application.DTOs.BookDtos;
using Application.Validators.BookValidators;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.IntegrationTests.BookIntegrationTests.CommandIntegrationTests
{
    public class UpdateBookIntegrationTest
    {
        private CleanArchitectureLibraryDb _dbContext;
        private CommandRepository<Book> _commandRepository;
        private QueryRepository<Book> _queryRepository;
        private QueryRepository<Author> _authorRepository;
        private IMapper _mapper;
        private UpdateBookDtoValidator _validator;
        private Mock<ILogger<UpdateBookCommandHandler>> _mockLogger;
        private UpdateBookCommandHandler _handler;

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
            _authorRepository = new QueryRepository<Author>(_dbContext);

            //Set up AutoMapper
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<Application.MappingProfiles.BookProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            //Initialize validator
            _validator = new UpdateBookDtoValidator();

            _mockLogger = new Mock<ILogger<UpdateBookCommandHandler>>();

            _handler = new UpdateBookCommandHandler(
                _commandRepository,
                _queryRepository,
                _authorRepository,
                _validator,
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
        public async Task Handle_ValidInput_UpdatesBookSuccessfully()
        {
            //Arrange
            var author = new Author { Id = Guid.NewGuid() };
            await _dbContext.Authors.AddAsync(author);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Original Title",
                Description = "Original Description",
                AuthorId = author.Id
            };
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();

            var updateBookDto = new UpdateBookDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                AuthorId = author.Id
            };

            var command = new UpdateBookCommand(book.Id, updateBookDto);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Book updated successfully."));

            var updatedBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            Assert.IsNotNull(updatedBook);
            Assert.That(updatedBook.Title, Is.EqualTo("Updated Title"));
            Assert.That(updatedBook.AuthorId, Is.EqualTo(author.Id));
        }

        [Test]
        public async Task Handle_BookDoesNotExist_ReturnsFailure()
        {
            //Arrange
            var updateBookDto = new UpdateBookDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                AuthorId = Guid.NewGuid()
            };

            var command = new UpdateBookCommand(Guid.NewGuid(), updateBookDto);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Error: Book not found."));
        }
    }
}
