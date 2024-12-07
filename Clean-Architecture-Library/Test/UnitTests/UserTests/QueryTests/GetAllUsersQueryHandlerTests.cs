using Application.DTOs.UserDtos;
using Application.Interfaces.RepositoryInterfaces;
using Application.Queries.Users.GetAllUsers;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.UnitTests.UserTests.QueryTests
{
    public class GetAllUsersQueryHandlerTests
    {
        private Mock<IQueryRepository<User>> _mockUserRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<GetAllUsersQueryHandler>> _mockLogger;
        private GetAllUsersQueryHandler _handler;
        private Mock<IMemoryCache> _mockMemoryCache;

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IQueryRepository<User>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetAllUsersQueryHandler>>();
            _mockMemoryCache = new Mock<IMemoryCache>();

            _handler = new GetAllUsersQueryHandler(
                _mockUserRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockMemoryCache.Object
                );
        }

        [Test]
        public async Task Handle_NoCache_ReturnsAllUsers()
        {
            //Arrange
            var users = new List<User> { new User(), new User() };
            var userDtos = new List<UserDto> { new UserDto(), new UserDto() };
            object cachedData = null!;
            var query = new GetAllUsersQuery();

            _mockMemoryCache.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out cachedData!))
                           .Returns(false);

            _mockMemoryCache.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                            .Returns(Mock.Of<ICacheEntry>);

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
            _mockMemoryCache.Verify(cache => cache.CreateEntry(It.IsAny<object>()), Times.Once);
        }

        [Test]
        public async Task Handle_CacheExists_ReturnsCachedUsers()
        {
            //Arrange
            var users = new List<User> { new User(), new User() };
            var userDtos = new List<UserDto> { new UserDto(), new UserDto() };
            object cachedData = users;
            var query = new GetAllUsersQuery();

            _mockMemoryCache.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out cachedData!))
                            .Returns(true);

            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<UserDto>>(users))
                       .Returns(userDtos);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.That(result, Is.EqualTo(userDtos));
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Never);
            _mockMapper.Verify(mapper => mapper.Map<IEnumerable<UserDto>>(users), Times.Once);
            _mockMemoryCache.Verify(cache => cache.TryGetValue(It.IsAny<object>(), out cachedData!), Times.Once);
        }
    }
}
