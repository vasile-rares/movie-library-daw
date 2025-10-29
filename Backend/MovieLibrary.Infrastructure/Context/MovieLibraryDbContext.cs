using Microsoft.EntityFrameworkCore;
using MovieLibrary.Domain.Entities;

namespace MovieLibrary.Infrastructure.Context
{
    public class MovieLibraryDbContext : DbContext
    {
        public MovieLibraryDbContext(DbContextOptions<MovieLibraryDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<ToWatchList> ToWatchList { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}