using Microsoft.EntityFrameworkCore;
using MovieLibrary.API.Data;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Repositories
{
  public class GenreRepository : IGenreRepository
  {
    private readonly MovieLibraryDbContext _context;

    public GenreRepository(MovieLibraryDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
      return await _context.Genres.ToListAsync();
    }

    public async Task<Genre?> GetByIdAsync(int id)
    {
      return await _context.Genres.FindAsync(id);
    }

    public async Task<Genre?> GetByNameAsync(string name)
    {
      return await _context.Genres.FirstOrDefaultAsync(g => g.Name == name);
    }

    public async Task<Genre> CreateAsync(Genre genre)
    {
      _context.Genres.Add(genre);
      await _context.SaveChangesAsync();
      return genre;
    }

    public async Task<Genre> UpdateAsync(Genre genre)
    {
      _context.Genres.Update(genre);
      await _context.SaveChangesAsync();
      return genre;
    }

    public async Task<bool> DeleteAsync(int id)
    {
      var genre = await _context.Genres.FindAsync(id);
      if (genre == null) return false;

      _context.Genres.Remove(genre);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
      return await _context.Genres.AnyAsync(g => g.Id == id);
    }
  }
}
