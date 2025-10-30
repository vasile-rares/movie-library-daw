using Microsoft.EntityFrameworkCore;
using MovieLibrary.API.Data;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Repositories
{
  public class ToWatchRepository : IToWatchRepository
  {
    private readonly MovieLibraryDbContext _context;

    public ToWatchRepository(MovieLibraryDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<ToWatchList>> GetAllAsync()
    {
      return await _context.ToWatchList
          .Include(t => t.User)
          .Include(t => t.Movie)
          .Include(t => t.Series)
          .ToListAsync();
    }

    public async Task<ToWatchList?> GetByIdAsync(int id)
    {
      return await _context.ToWatchList
          .Include(t => t.User)
          .Include(t => t.Movie)
          .Include(t => t.Series)
          .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<ToWatchList>> GetByUserIdAsync(int userId)
    {
      return await _context.ToWatchList
          .Include(t => t.User)
          .Include(t => t.Movie)
          .Include(t => t.Series)
          .Where(t => t.UserId == userId)
          .ToListAsync();
    }

    public async Task<ToWatchList> CreateAsync(ToWatchList toWatch)
    {
      _context.ToWatchList.Add(toWatch);
      await _context.SaveChangesAsync();
      return toWatch;
    }

    public async Task<bool> DeleteAsync(int id)
    {
      var toWatch = await _context.ToWatchList.FindAsync(id);
      if (toWatch == null) return false;

      _context.ToWatchList.Remove(toWatch);
      await _context.SaveChangesAsync();
      return true;
    }
  }
}
