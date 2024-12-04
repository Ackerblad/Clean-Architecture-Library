using Application.DTOs.BookDtos;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateBookDto, Book>();
        }
    }
}
