//using Application.Commands.Books.CreateBook;
//using Infrastructure;

//namespace Test.BookTests.CommandTests
//{
//    public class CreateBookCommandHandlerTests
//    {
//        private FakeDatabase _fakeDatabase;
//        private CreateBookCommandHandler _handler;

//        [SetUp]
//        public void SetUp()
//        {
//            _fakeDatabase = new FakeDatabase();
//            _handler = new CreateBookCommandHandler(_fakeDatabase);
//        }

//        [Test]
//        public async Task Handle_AuthorExists_AddsBookToDatabase()
//        {
//            //Arrange
//            var initialBooks = _fakeDatabase.Books.Count;
//            var command = new CreateBookCommand("AuthorExists", "Description", 1);

//            //Act
//            var createdBook = await _handler.Handle(command, CancellationToken.None);

//            //Assert
//            Assert.That(createdBook.Title, Is.EqualTo("AuthorExists"));
//            Assert.That(_fakeDatabase.Books.Count, Is.EqualTo(initialBooks + 1));
//        }

//        [Test]
//        public void Handle_AuthorDoesNotExist_ThrowsKeyNotFoundException()
//        {
//            //Arrange
//            var initialBooks = _fakeDatabase.Books.Count;
//            var command = new CreateBookCommand("AuthorDoesNotExist", "Description", 99);

//            //Act
//            Task action() => _handler.Handle(command, CancellationToken.None);

//            //Assert
//            Assert.ThrowsAsync<KeyNotFoundException>(action);
//            Assert.That(_fakeDatabase.Books.Count, Is.EqualTo(initialBooks));
//        }
//    }
//}
