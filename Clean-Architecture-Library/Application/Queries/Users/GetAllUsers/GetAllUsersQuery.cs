using Application.DTOs.UserDtos;
using MediatR;

namespace Application.Queries.Users.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>{}
}
