using Application.Queries.Users.LoginUser;
using Domain.Entities;
using Application.Interfaces.RepositoryInterfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Application.Interfaces.HelperInterfaces;

namespace Test.UserTests.QueryTests
{
    public class LoginUserQueryHandlerTests
    {
        private Mock<IQueryRepository<User>> _mockUserRepository;
        private Mock<ITokenHelper> _mockTokenHelper;
        private Mock<ILogger<LoginUserQueryHandler>> _mockLogger;
        private LoginUserQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IQueryRepository<User>>();
            _mockTokenHelper = new Mock<ITokenHelper>();
            _mockLogger = new Mock<ILogger<LoginUserQueryHandler>>();

            _handler = new LoginUserQueryHandler(
                _mockUserRepository.Object,
                _mockTokenHelper.Object,
                _mockLogger.Object
                );
        }

        [Test]
        public async Task Handle_ValidCredentials_GeneratesToken()
        {
            //Arrange
            var userName = "validUser";
            var password = "validPassword";
            var user = new User { Id = Guid.NewGuid(), Username = userName, Password = password };
            var token = "generatedToken";
            var query = new LoginUserQuery(userName, password);

            _mockUserRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(new List<User> { user });

            _mockTokenHelper.Setup(helper => helper.GenerateJwtToken(user))
                            .Returns(token);


            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Login successful."));
            Assert.That(result.Data, Is.EqualTo(token));
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockTokenHelper.Verify(helper => helper.GenerateJwtToken(user), Times.Once);
        }

        [Test]
        public async Task Handle_InvalidCredentials_ReturnsFailureOperationResult()
        {
            //Arrange
            var userName = "invalidUser";
            var password = "invalidPassword";
            var query = new LoginUserQuery(userName, password);

            _mockUserRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(new List<User>());

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Is.EqualTo("Unauthorized"));
            Assert.That(result.ErrorMessage, Is.EqualTo("Invalid username or password."));
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockTokenHelper.Verify(helper => helper.GenerateJwtToken(It.IsAny<User>()), Times.Never);
        }
    }
}
