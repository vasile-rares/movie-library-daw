using Microsoft.EntityFrameworkCore;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Data
{
    public class MovieLibraryDbContext : DbContext
    {
        public MovieLibraryDbContext(DbContextOptions<MovieLibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<SeriesGenre> SeriesGenres { get; set; }
        public DbSet<ToWatchList> ToWatchList { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite primary key for MovieGenres
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            // Configure composite primary key for SeriesGenres
            modelBuilder.Entity<SeriesGenre>()
                .HasKey(sg => new { sg.SeriesId, sg.GenreId });

            // Configure many-to-many relationship: Movies <-> Genres
            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure many-to-many relationship: Series <-> Genres
            modelBuilder.Entity<SeriesGenre>()
                .HasOne(sg => sg.Series)
                .WithMany(s => s.SeriesGenres)
                .HasForeignKey(sg => sg.SeriesId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SeriesGenre>()
                .HasOne(sg => sg.Genre)
                .WithMany(g => g.SeriesGenres)
                .HasForeignKey(sg => sg.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Genre>()
                .HasIndex(g => g.Name)
                .IsUnique();
        }
    }
}