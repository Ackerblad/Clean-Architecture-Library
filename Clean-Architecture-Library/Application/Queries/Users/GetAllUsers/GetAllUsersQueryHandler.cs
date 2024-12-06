using Application.DTOs.UserDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Users.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IQueryRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;
        private readonly IMemoryCache _memoryCache;
        private const string cacheKey = "allUsersCacheKey";

        public GetAllUsersQueryHandler(IQueryRepository<User> userRepository, IMapper mapper, ILogger<GetAllUsersQueryHandler> logger, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllUsersQuery");

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<User> allUsers))
            {
                _logger.LogInformation("Cache miss. Retrieving users from database.");

                allUsers = await _userRepository.GetAllAsync();
                _memoryCache.Set(cacheKey, allUsers, TimeSpan.FromMinutes(5));

                _logger.LogInformation("User data cached successfully.");
            }
            
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(allUsers);
            _logger.LogInformation("{Count} users retrieved", userDtos.Count());

            return userDtos;
        }
    }
}
