using Application.Commands.Books.CreateBook;
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
    public class CreateBookIntegrationTest
    {
        private CleanArchitectureLibraryDb _dbContext;
        private CommandRepository<Book> _bookRepository;
        private QueryRepository<Author> _authorRepository;
        private IMapper _mapper;
        private CreateBookDtoValidator _validator;
        private Mock<ILogger<CreateBookCommandHandler>> _mockLogger;
        private CreateBookCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            //Configure InMemoryDatabase
            var options = new DbContextOptionsBuilder<CleanArchitectureLibraryDb>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _dbContext = new CleanArchitectureLibraryDb(options);

            //Initialize repositories with InMemoryDatabase
            _bookRepository = new CommandRepository<Book>(_dbContext);
            _authorRepository = new QueryRepository<Author>(_dbContext);

            //Set up AutoMapper
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<Application.MappingProfiles.BookProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            //Initialize validator
            _validator = new CreateBookDtoValidator();

            _mockLogger = new Mock<ILogger<CreateBookCommandHandler>>();

            _handler = new CreateBookCommandHandler(
                _bookRepository,
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
        public async Task Handle_ValidInput_CreatesBookSuccessfully()
        {
            //Arrange
            var author = new Author { Id = Guid.NewGuid() };
            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();

            var createBookDto = new CreateBookDto
            {
                Title = "Test Book",
                Description = "Test Description",
                AuthorId = author.Id
            };

            var command = new CreateBookCommand(createBookDto);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Book created successfully."));

            var createdBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Title == "Test Book");
            Assert.IsNotNull(createdBook);
            Assert.That(createdBook.Description, Is.EqualTo("Test Description"));
            Assert.That(createdBook.AuthorId, Is.EqualTo(author.Id));
        }

        [Test]
        public async Task Handle_InvalidInput_ReturnsValidationFailure()
        {
            //Arrange
            var createBookDto = new CreateBookDto
            {
                Title = "",
                Description = "A test description",
                AuthorId = Guid.NewGuid()
            };

            var command = new CreateBookCommand(createBookDto);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Validation failed."));
            Assert.That(result.ErrorMessage, Is.EqualTo("Title is required."));
        }
    }
}
