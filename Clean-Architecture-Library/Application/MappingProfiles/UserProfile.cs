using Application.DTOs.UserDtos;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto,User>();
            CreateMap<UpdateUserDto, User>();
        }
    }
}
