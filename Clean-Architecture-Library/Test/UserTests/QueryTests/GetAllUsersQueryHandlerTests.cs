//using Application.Queries.Users.GetAllUsers;
//using Infrastructure;

//namespace Test.UserTests.QueryTests
//{
//    public class GetAllUsersQueryHandlerTests
//    {
//        private FakeDatabase _fakeDatabase;
//        private GetAllUsersQueryHandler _handler;

//        [SetUp]
//        public void SetUp()
//        {
//            _fakeDatabase = new FakeDatabase();
//            _handler = new GetAllUsersQueryHandler(_fakeDatabase);
//        }

//        [Test]
//        public async Task Handle_ReturnsAllUsers()
//        {
//            //Arrange
//            var query = new GetAllUsersQuery();

//            //Act
//            var result = await _handler.Handle(query, CancellationToken.None);

//            //Assert
//            Assert.That(result, Is.Not.Null);
//            Assert.That(result.Count, Is.EqualTo(_fakeDatabase.Users.Count));
//        }
//    }
//}
