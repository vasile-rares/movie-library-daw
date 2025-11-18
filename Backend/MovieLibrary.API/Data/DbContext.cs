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
    public DbSet<Title> Titles { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<TitleGenre> TitleGenres { get; set; }
    public DbSet<MyList> MyLists { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Configure composite primary key for TitleGenres
      modelBuilder.Entity<TitleGenre>()
          .HasKey(tg => new { tg.TitleId, tg.GenreId });

      // Configure many-to-many relationship: Titles <-> Genres
      modelBuilder.Entity<TitleGenre>()
          .HasOne(tg => tg.Title)
          .WithMany(t => t.TitleGenres)
          .HasForeignKey(tg => tg.TitleId)
          .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<TitleGenre>()
          .HasOne(tg => tg.Genre)
          .WithMany(g => g.TitleGenres)
          .HasForeignKey(tg => tg.GenreId)
          .OnDelete(DeleteBehavior.Cascade);

      // Configure unique constraint for MyList (User can have same Title only once)
      modelBuilder.Entity<MyList>()
          .HasIndex(ml => new { ml.UserId, ml.TitleId })
          .IsUnique();

      // Configure unique constraints
      modelBuilder.Entity<User>()
          .HasIndex(u => u.Nickname)
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
