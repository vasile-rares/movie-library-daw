using Microsoft.EntityFrameworkCore;
using MovieLibrary.API.Data;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Repositories
{
  public class RatingRepository : IRatingRepository
  {
    private readonly MovieLibraryDbContext _context;

    public RatingRepository(MovieLibraryDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Rating>> GetAllAsync()
    {
      return await _context.Ratings
          .Include(r => r.User)
          .Include(r => r.Title)
          .ToListAsync();
    }

    public async Task<Rating?> GetByIdAsync(int id)
    {
      return await _context.Ratings
          .Include(r => r.User)
          .Include(r => r.Title)
          .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Rating>> GetByUserIdAsync(int userId)
    {
      return await _context.Ratings
          .Include(r => r.Title)
          .Where(r => r.UserId == userId)
          .ToListAsync();
    }

    public async Task<IEnumerable<Rating>> GetByTitleIdAsync(int titleId)
    {
      return await _context.Ratings
          .Include(r => r.User)
          .Where(r => r.TitleId == titleId)
          .ToListAsync();
    }

    public async Task<Rating> CreateAsync(Rating rating)
    {
      _context.Ratings.Add(rating);
      await _context.SaveChangesAsync();
      return rating;
    }

    public async Task<Rating> UpdateAsync(Rating rating)
    {
      _context.Ratings.Update(rating);
      await _context.SaveChangesAsync();
      return rating;
    }

    public async Task<bool> DeleteAsync(int id)
    {
      var rating = await _context.Ratings.FindAsync(id);
      if (rating == null) return false;

      _context.Ratings.Remove(rating);
      await _context.SaveChangesAsync();
      return true;
    }
  }
}
