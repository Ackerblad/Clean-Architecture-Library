using Application.DTOs.UserDtos;
using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Users.GetAllUsers;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.UserTests.QueryTests
{
    public class GetAllUsersQueryHandlerTests
    {
        private Mock<IQueryRepository<User>> _mockUserRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<GetAllUsersQueryHandler>> _mockLogger;
        private GetAllUsersQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IQueryRepository<User>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetAllUsersQueryHandler>>();

            _handler = new GetAllUsersQueryHandler(
                _mockUserRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object
                );
        }

        [Test]
        public async Task Handle_ReturnsAllUsers()
        {
            //Arrange
            var users = new List<User> { new User(), new User() };
            var userDtos = new List<UserDto> { new UserDto(), new UserDto() };
            var query = new GetAllUsersQuery();

            _mockUserRepository.Setup(repo => repo.GetAllAsync())
                               .ReturnsAsync(users);

            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<UserDto>>(users))
                       .Returns(userDtos);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.That(result, Is.EqualTo(userDtos));
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<IEnumerable<UserDto>>(users), Times.Once);
        }
    }
}
