using Microsoft.EntityFrameworkCore;
using MovieLibrary.API.Data;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Repositories
{
  public class SeriesRepository : ISeriesRepository
  {
    private readonly MovieLibraryDbContext _context;

    public SeriesRepository(MovieLibraryDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Series>> GetAllAsync()
    {
      return await _context.Series
          .Include(s => s.SeriesGenres)
              .ThenInclude(sg => sg.Genre)
          .ToListAsync();
    }

    public async Task<Series?> GetByIdAsync(int id)
    {
      return await _context.Series
          .Include(s => s.SeriesGenres)
              .ThenInclude(sg => sg.Genre)
          .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Series>> GetByGenreAsync(int genreId)
    {
      return await _context.Series
          .Include(s => s.SeriesGenres)
              .ThenInclude(sg => sg.Genre)
          .Where(s => s.SeriesGenres.Any(sg => sg.GenreId == genreId))
          .ToListAsync();
    }

    public async Task<Series> CreateAsync(Series series)
    {
      _context.Series.Add(series);
      await _context.SaveChangesAsync();
      return series;
    }

    public async Task<Series> UpdateAsync(Series series)
    {
      _context.Series.Update(series);
      await _context.SaveChangesAsync();
      return series;
    }

    public async Task<bool> DeleteAsync(int id)
    {
      var series = await _context.Series.FindAsync(id);
      if (series == null) return false;

      _context.Series.Remove(series);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
      return await _context.Series.AnyAsync(s => s.Id == id);
    }

    public async Task AddGenresToSeriesAsync(int seriesId, List<int> genreIds)
    {
      var seriesGenres = genreIds.Select(genreId => new SeriesGenre
      {
        SeriesId = seriesId,
        GenreId = genreId
      });

      _context.SeriesGenres.AddRange(seriesGenres);
      await _context.SaveChangesAsync();
    }

    public async Task RemoveGenresFromSeriesAsync(int seriesId)
    {
      var seriesGenres = await _context.SeriesGenres
          .Where(sg => sg.SeriesId == seriesId)
          .ToListAsync();

      _context.SeriesGenres.RemoveRange(seriesGenres);
      await _context.SaveChangesAsync();
    }
  }
}
