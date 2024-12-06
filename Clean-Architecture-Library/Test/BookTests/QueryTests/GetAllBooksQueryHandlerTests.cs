using Application.DTOs.BookDtos;
using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Books.GetAllBooks;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.BookTests.QueryTests
{
    public class GetAllBooksQueryHandlerTests
    {
        private Mock<IQueryRepository<Book>> _mockBookRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<GetAllBooksQueryHandler>> _mockLogger;
        private GetAllBooksQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockBookRepository = new Mock<IQueryRepository<Book>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetAllBooksQueryHandler>>();

            _handler = new GetAllBooksQueryHandler(
                _mockBookRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object
                );
        }

        [Test]
        public async Task Handle_ReturnsAllBooks()
        {
            //Arrange
            var books = new List<Book> { new Book(), new Book() };
            var bookDtos = new List<BookDto> { new BookDto(), new BookDto() };
            var query = new GetAllBooksQuery();

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
        }
    }
}
