using Microsoft.EntityFrameworkCore;
using MovieLibrary.API.Data;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Repositories
{
  public class MovieRepository : IMovieRepository
  {
    private readonly MovieLibraryDbContext _context;

    public MovieRepository(MovieLibraryDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
      return await _context.Movies
          .Include(m => m.MovieGenres)
              .ThenInclude(mg => mg.Genre)
          .ToListAsync();
    }

    public async Task<Movie?> GetByIdAsync(int id)
    {
      return await _context.Movies
          .Include(m => m.MovieGenres)
              .ThenInclude(mg => mg.Genre)
          .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Movie>> GetByGenreAsync(int genreId)
    {
      return await _context.Movies
          .Include(m => m.MovieGenres)
              .ThenInclude(mg => mg.Genre)
          .Where(m => m.MovieGenres.Any(mg => mg.GenreId == genreId))
          .ToListAsync();
    }

    public async Task<Movie> CreateAsync(Movie movie)
    {
      _context.Movies.Add(movie);
      await _context.SaveChangesAsync();
      return movie;
    }

    public async Task<Movie> UpdateAsync(Movie movie)
    {
      _context.Movies.Update(movie);
      await _context.SaveChangesAsync();
      return movie;
    }

    public async Task<bool> DeleteAsync(int id)
    {
      var movie = await _context.Movies.FindAsync(id);
      if (movie == null) return false;

      _context.Movies.Remove(movie);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
      return await _context.Movies.AnyAsync(m => m.Id == id);
    }

    public async Task AddGenresToMovieAsync(int movieId, List<int> genreIds)
    {
      var movieGenres = genreIds.Select(genreId => new MovieGenre
      {
        MovieId = movieId,
        GenreId = genreId
      });

      _context.MovieGenres.AddRange(movieGenres);
      await _context.SaveChangesAsync();
    }

    public async Task RemoveGenresFromMovieAsync(int movieId)
    {
      var movieGenres = await _context.MovieGenres
          .Where(mg => mg.MovieId == movieId)
          .ToListAsync();

      _context.MovieGenres.RemoveRange(movieGenres);
      await _context.SaveChangesAsync();
    }
  }
}
