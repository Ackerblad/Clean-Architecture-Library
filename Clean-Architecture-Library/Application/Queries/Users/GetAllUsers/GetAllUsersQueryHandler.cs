using Application.DTOs.UserDtos;
using Application.Interfaces.RepositoryInterfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Users.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IQueryRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;

        public GetAllUsersQueryHandler(IQueryRepository<User> userRepository, IMapper mapper, ILogger<GetAllUsersQueryHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllUsersQuery");
            var users = await _userRepository.GetAllAsync();

            _logger.LogInformation("{Count} users retrieved", users.Count());
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
