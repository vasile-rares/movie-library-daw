using AutoMapper;
using MovieLibrary.API.DTOs.Responses.Movie;
using MovieLibrary.API.DTOs.Responses.Series;
using MovieLibrary.API.DTOs.Responses.Rating;
using MovieLibrary.API.DTOs.Responses.ToWatch;
using MovieLibrary.API.DTOs.Responses.Genre;
using MovieLibrary.API.DTOs.Responses.User;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Movie mappings
        CreateMap<Movie, MovieResponseDto>()
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                src.MovieGenres.Select(mg => new GenreResponseDto
                {
                    Id = mg.Genre.Id,
                    Name = mg.Genre.Name
                })));

        // Series mappings
        CreateMap<Series, SeriesResponseDto>()
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                src.SeriesGenres.Select(sg => new GenreResponseDto
                {
                    Id = sg.Genre.Id,
                    Name = sg.Genre.Name
                })));

        // Rating mappings (flattened structure)
        CreateMap<Rating, RatingResponseDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Movie != null ? src.Movie.Title : null))
            .ForMember(dest => dest.SeriesTitle, opt => opt.MapFrom(src => src.Series != null ? src.Series.Title : null));

        // ToWatchList mappings (flattened structure)
        CreateMap<ToWatchList, ToWatchResponseDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.Username : "Unknown"))
            .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Movie != null ? src.Movie.Title : null))
            .ForMember(dest => dest.MovieImageUrl, opt => opt.MapFrom(src => src.Movie != null ? src.Movie.ImageUrl : null))
            .ForMember(dest => dest.SeriesTitle, opt => opt.MapFrom(src => src.Series != null ? src.Series.Title : null))
            .ForMember(dest => dest.SeriesImageUrl, opt => opt.MapFrom(src => src.Series != null ? src.Series.ImageUrl : null));

        // Genre mappings
        CreateMap<Genre, GenreResponseDto>();

        // User mappings
        CreateMap<User, UserResponseDto>();
    }
}
