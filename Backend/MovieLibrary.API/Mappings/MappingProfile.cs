using AutoMapper;
using MovieLibrary.API.DTOs.Responses.Title;
using MovieLibrary.API.DTOs.Responses.Rating;
using MovieLibrary.API.DTOs.Responses.MyList;
using MovieLibrary.API.DTOs.Responses.Genre;
using MovieLibrary.API.DTOs.Responses.User;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Title mappings
        CreateMap<Title, TitleResponseDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                src.TitleGenres.Select(tg => new GenreResponseDto
                {
                    Id = tg.Genre.Id,
                    Name = tg.Genre.Name
                })));

        // Rating mappings (flattened structure)
        CreateMap<Rating, RatingResponseDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Nickname))
            .ForMember(dest => dest.TitleName, opt => opt.MapFrom(src => src.Title.Name))
            .ForMember(dest => dest.TitleType, opt => opt.MapFrom(src => src.Title.Type));

        // MyList mappings (flattened structure)
        CreateMap<MyList, MyListResponseDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Nickname))
            .ForMember(dest => dest.TitleName, opt => opt.MapFrom(src => src.Title.Name))
            .ForMember(dest => dest.TitleType, opt => opt.MapFrom(src => src.Title.Type))
            .ForMember(dest => dest.TitleImageUrl, opt => opt.MapFrom(src => src.Title.ImageUrl));

        // Genre mappings
        CreateMap<Genre, GenreResponseDto>();

        // User mappings
        CreateMap<User, UserResponseDto>();
    }
}
