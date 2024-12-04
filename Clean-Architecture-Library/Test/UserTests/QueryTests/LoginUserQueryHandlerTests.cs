//using Application.Queries.Users.LoginUser.Helpers;
//using Application.Queries.Users.LoginUser;
//using Infrastructure;
//using Microsoft.Extensions.Configuration;
//using Domain.Entities;

//namespace Test.UserTests.QueryTests
//{
//    public class LoginUserQueryHandlerTests
//    {
//        private FakeDatabase _fakeDatabase;
//        private TokenHelper _tokenHelper;
//        private LoginUserQueryHandler _handler;

//        [SetUp]
//        public void SetUp()
//        {
//            _fakeDatabase = new FakeDatabase
//            {
//                Users = new List<User>
//                {
//                    new User(1, "TestUser", "TestPassword"),
//                }
//            };

//            var configuration = new ConfigurationBuilder()
//                .AddInMemoryCollection(new Dictionary<string, string>
//                {
//                    { "JwtSettings:SecretKey", "SuperSecureSecretTestKeyGreaterThan256" }
//                })
//                .Build();

//            _tokenHelper = new TokenHelper(configuration);
//            _handler = new LoginUserQueryHandler(_fakeDatabase, _tokenHelper);
//        }

//        [Test]
//        public async Task Handle_ValidCredentials_GeneratesToken()
//        {
//            //Arrange
//            var query = new LoginUserQuery("TestUser", "TestPassword");

//            //Act
//            var result = await _handler.Handle(query, CancellationToken.None);

//            //Assert
//            Assert.That(result, Is.Not.Null.Or.Empty);
//        }

//        [Test]
//        public async Task Handle_InvalidCredentials_ThrowsUnauthorizedAccessException()
//        {
//            //Arrange
//            var query = new LoginUserQuery("InvalidUser", "InvalidPassword");

//            //Act
//            Task action() => _handler.Handle(query, CancellationToken.None);

//            //Assert
//            Assert.ThrowsAsync<UnauthorizedAccessException>(action);
//        }
//    }
//}
