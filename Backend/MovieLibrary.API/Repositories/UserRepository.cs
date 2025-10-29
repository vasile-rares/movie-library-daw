using Microsoft.EntityFrameworkCore;
using MovieLibrary.API.Data;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly MovieLibraryDbContext _context;

    public UserRepository(MovieLibraryDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
      return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
      return await _context.Users
          .Include(u => u.Ratings)
          .Include(u => u.ToWatchList)
          .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
      return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
      return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> CreateAsync(User user)
    {
      _context.Users.Add(user);
      await _context.SaveChangesAsync();
      return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
      _context.Users.Update(user);
      await _context.SaveChangesAsync();
      return user;
    }

    public async Task<bool> DeleteAsync(int id)
    {
      var user = await _context.Users.FindAsync(id);
      if (user == null) return false;

      _context.Users.Remove(user);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
      return await _context.Users.AnyAsync(u => u.Id == id);
    }
  }
}
