using Application.DTOs.BookDtos;
using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Books.GetBookById;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.BookTests.QueryTests
{
    public class GetBookByIdQueryHandlerTests
    {
        private Mock<IQueryRepository<Book>> _mockBookRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<GetBookByIdQueryHandler>> _mockLogger;
        private GetBookByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockBookRepository = new Mock<IQueryRepository<Book>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetBookByIdQueryHandler>>();

            _handler = new GetBookByIdQueryHandler(
                _mockBookRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object
                );
        }

        [Test]
        public async Task Handle_BookExists_ReturnsSuccessOperationResult()
        {
            //Arrange
            var bookId = Guid.NewGuid();
            var book = new Book { Id = bookId };
            var bookDto = new BookDto { Id = bookId };
            var query = new GetBookByIdQuery(bookId);

            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId))
                               .ReturnsAsync(book);

            _mockMapper.Setup(mapper => mapper.Map<BookDto>(book))
                       .Returns(bookDto);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Book retrieved successfully."));
            Assert.That(result.Data, Is.EqualTo(bookDto));
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(bookId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<BookDto>(book), Times.Once);
        }

        [Test]
        public async Task Handle_BookDoesNotExist_ReturnsFailureOperationResult()
        {
            //Arrange
            var bookId = Guid.NewGuid();
            var query = new GetBookByIdQuery(bookId);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Error: Book Not Found"));
            _mockBookRepository.Verify(repo => repo.GetByIdAsync(bookId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<BookDto>(It.IsAny<Book>()), Times.Never);
        }
    }
}
