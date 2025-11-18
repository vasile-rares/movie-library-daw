using Microsoft.EntityFrameworkCore;
using MovieLibrary.API.Data;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Repositories
{
    public class MyListRepository : IMyListRepository
    {
        private readonly MovieLibraryDbContext _context;

        public MyListRepository(MovieLibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MyList>> GetAllAsync()
        {
            return await _context.MyLists
                .Include(m => m.User)
                .Include(m => m.Title)
                .ToListAsync();
        }

        public async Task<MyList?> GetByIdAsync(int id)
        {
            return await _context.MyLists
                .Include(m => m.User)
                .Include(m => m.Title)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MyList>> GetByUserIdAsync(int userId)
        {
            return await _context.MyLists
                .Include(m => m.User)
                .Include(m => m.Title)
                .Where(m => m.UserId == userId)
                .ToListAsync();
        }

        public async Task<MyList?> GetByUserAndTitleAsync(int userId, int titleId)
        {
            return await _context.MyLists
                .FirstOrDefaultAsync(m => m.UserId == userId && m.TitleId == titleId);
        }

        public async Task<MyList> CreateAsync(MyList myList)
        {
            _context.MyLists.Add(myList);
            await _context.SaveChangesAsync();
            return myList;
        }

        public async Task<MyList> UpdateAsync(MyList myList)
        {
            _context.MyLists.Update(myList);
            await _context.SaveChangesAsync();
            return myList;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var myList = await _context.MyLists.FindAsync(id);
            if (myList == null) return false;

            _context.MyLists.Remove(myList);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
