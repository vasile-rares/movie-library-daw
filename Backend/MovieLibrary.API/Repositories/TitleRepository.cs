using Microsoft.EntityFrameworkCore;
using MovieLibrary.API.Data;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Repositories
{
    public class TitleRepository : ITitleRepository
    {
        private readonly MovieLibraryDbContext _context;

        public TitleRepository(MovieLibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Title>> GetAllAsync()
        {
            return await _context.Titles
                .Include(t => t.TitleGenres)
                    .ThenInclude(tg => tg.Genre)
                .ToListAsync();
        }

        public async Task<Title?> GetByIdAsync(int id)
        {
            return await _context.Titles
                .Include(t => t.TitleGenres)
                    .ThenInclude(tg => tg.Genre)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Title>> GetByTypeAsync(TitleType type)
        {
            return await _context.Titles
                .Include(t => t.TitleGenres)
                    .ThenInclude(tg => tg.Genre)
                .Where(t => t.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<Title>> GetByGenreAsync(int genreId)
        {
            return await _context.Titles
                .Include(t => t.TitleGenres)
                    .ThenInclude(tg => tg.Genre)
                .Where(t => t.TitleGenres.Any(tg => tg.GenreId == genreId))
                .ToListAsync();
        }

        public async Task<Title> CreateAsync(Title title)
        {
            _context.Titles.Add(title);
            await _context.SaveChangesAsync();
            return title;
        }

        public async Task<Title> UpdateAsync(Title title)
        {
            _context.Titles.Update(title);
            await _context.SaveChangesAsync();
            return title;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var title = await _context.Titles.FindAsync(id);
            if (title == null) return false;

            _context.Titles.Remove(title);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Titles.AnyAsync(t => t.Id == id);
        }

        public async Task AddGenresToTitleAsync(int titleId, List<int> genreIds)
        {
            var titleGenres = genreIds.Select(genreId => new TitleGenre
            {
                TitleId = titleId,
                GenreId = genreId
            });

            _context.TitleGenres.AddRange(titleGenres);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveGenresFromTitleAsync(int titleId)
        {
            var titleGenres = await _context.TitleGenres
                .Where(tg => tg.TitleId == titleId)
                .ToListAsync();

            _context.TitleGenres.RemoveRange(titleGenres);
            await _context.SaveChangesAsync();
        }
    }
}
