using MovieLibrary.API.Models;

namespace MovieLibrary.API.Data
{
    public static class DbSeeder
    {
        public static void SeedData(MovieLibraryDbContext context)
        {
            // Check if data already exists
            if (context.Users.Any() || context.Genres.Any() || context.Movies.Any())
            {
                return; // Database has been seeded
            }

            // Seed Genres
            var genres = new List<Genre>
            {
                new Genre { Name = "Action" },
                new Genre { Name = "Comedy" },
                new Genre { Name = "Drama" },
                new Genre { Name = "Horror" },
                new Genre { Name = "Sci-Fi" },
                new Genre { Name = "Romance" },
                new Genre { Name = "Thriller" },
                new Genre { Name = "Animation" },
                new Genre { Name = "Documentary" },
                new Genre { Name = "Fantasy" }
            };
            context.Genres.AddRange(genres);
            context.SaveChanges();

            // Seed Users (password for all is: "password123")
            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    Email = "admin@movielibrary.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), // password123
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Username = "john_doe",
                    Email = "john.doe@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), // password123
                    Role = "User",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Username = "jane_smith",
                    Email = "jane@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), // password123
                    Role = "User",
                    CreatedAt = DateTime.UtcNow
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            // Seed Movies
            var movies = new List<Movie>
            {
                new Movie
                {
                    Title = "The Matrix",
                    Description = "A computer hacker learns about the true nature of his reality.",
                    ReleaseYear = 1999,
                    ImageUrl = "https://image.tmdb.org/t/p/original/p96dm7sCMn4VYAStA6siNz30G1r.jpg"
                },
                new Movie
                {
                    Title = "Inception",
                    Description = "A thief who steals corporate secrets through dream-sharing technology.",
                    ReleaseYear = 2010,
                    ImageUrl = "https://image.tmdb.org/t/p/original/ljsZTbVsrQSqZgWeep2B1QiDKuh.jpg"
                },
                new Movie
                {
                    Title = "The Godfather",
                    Description = "The aging patriarch of an organized crime dynasty transfers control.",
                    ReleaseYear = 1972,
                    ImageUrl = "https://image.tmdb.org/t/p/original/3bhkrj58Vtu7enYsRolD1fZdja1.jpg"
                },
                new Movie
                {
                    Title = "Pulp Fiction",
                    Description = "The lives of two mob hitmen, a boxer, and a pair of diner bandits intertwine.",
                    ReleaseYear = 1994,
                    ImageUrl = "https://image.tmdb.org/t/p/original/vQWk5YBFWF4bZaofAbv0tShwBvQ.jpg"
                },
                new Movie
                {
                    Title = "Interstellar",
                    Description = "A team of explorers travel through a wormhole in space.",
                    ReleaseYear = 2014,
                    ImageUrl = "https://image.tmdb.org/t/p/original/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg"
                }
            };
            context.Movies.AddRange(movies);
            context.SaveChanges();

            // Seed Series
            var series = new List<Series>
            {
                new Series
                {
                    Title = "Breaking Bad",
                    Description = "A high school chemistry teacher turned meth producer.",
                    ReleaseYear = 2008,
                    SeasonsCount = 5,
                    EpisodesCount = 62,
                    ImageUrl = "https://image.tmdb.org/t/p/original/ztkUQFLlC19CCMYHW9o1zWhJRNq.jpg"
                },
                new Series
                {
                    Title = "Game of Thrones",
                    Description = "Nine noble families fight for control over the lands of Westeros.",
                    ReleaseYear = 2011,
                    SeasonsCount = 8,
                    EpisodesCount = 73,
                    ImageUrl = "https://image.tmdb.org/t/p/original/1XS1oqL89opfnbLl8WnZY1O1uJx.jpg"
                },
                new Series
                {
                    Title = "Stranger Things",
                    Description = "When a young boy disappears, his mother and friends uncover supernatural mysteries.",
                    ReleaseYear = 2016,
                    SeasonsCount = 4,
                    EpisodesCount = 42,
                    ImageUrl = "https://image.tmdb.org/t/p/original/uOOtwVbSr4QDjAGIifLDwpb2Pdl.jpg"
                },
                new Series
                {
                    Title = "The Office",
                    Description = "A mockumentary on a group of typical office workers.",
                    ReleaseYear = 2005,
                    SeasonsCount = 9,
                    EpisodesCount = 201,
                    ImageUrl = "https://image.tmdb.org/t/p/original/7DJKHzAi83BmQrWLrYYOqcoKfhR.jpg"
                }
            };
            context.Series.AddRange(series);
            context.SaveChanges();

            // Seed MovieGenres (Many-to-Many)
            var movieGenres = new List<MovieGenre>
            {
                // The Matrix - Action, Sci-Fi
                new MovieGenre { MovieId = movies[0].Id, GenreId = genres[0].Id },
                new MovieGenre { MovieId = movies[0].Id, GenreId = genres[4].Id },
                
                // Inception - Action, Sci-Fi, Thriller
                new MovieGenre { MovieId = movies[1].Id, GenreId = genres[0].Id },
                new MovieGenre { MovieId = movies[1].Id, GenreId = genres[4].Id },
                new MovieGenre { MovieId = movies[1].Id, GenreId = genres[6].Id },
                
                // The Godfather - Drama
                new MovieGenre { MovieId = movies[2].Id, GenreId = genres[2].Id },
                
                // Pulp Fiction - Drama, Thriller
                new MovieGenre { MovieId = movies[3].Id, GenreId = genres[2].Id },
                new MovieGenre { MovieId = movies[3].Id, GenreId = genres[6].Id },
                
                // Interstellar - Sci-Fi, Drama
                new MovieGenre { MovieId = movies[4].Id, GenreId = genres[4].Id },
                new MovieGenre { MovieId = movies[4].Id, GenreId = genres[2].Id }
            };
            context.MovieGenres.AddRange(movieGenres);
            context.SaveChanges();

            // Seed SeriesGenres (Many-to-Many)
            var seriesGenres = new List<SeriesGenre>
            {
                // Breaking Bad - Drama, Thriller
                new SeriesGenre { SeriesId = series[0].Id, GenreId = genres[2].Id },
                new SeriesGenre { SeriesId = series[0].Id, GenreId = genres[6].Id },
                
                // Game of Thrones - Drama, Fantasy
                new SeriesGenre { SeriesId = series[1].Id, GenreId = genres[2].Id },
                new SeriesGenre { SeriesId = series[1].Id, GenreId = genres[9].Id },
                
                // Stranger Things - Horror, Sci-Fi, Thriller
                new SeriesGenre { SeriesId = series[2].Id, GenreId = genres[3].Id },
                new SeriesGenre { SeriesId = series[2].Id, GenreId = genres[4].Id },
                new SeriesGenre { SeriesId = series[2].Id, GenreId = genres[6].Id },
                
                // The Office - Comedy
                new SeriesGenre { SeriesId = series[3].Id, GenreId = genres[1].Id }
            };
            context.SeriesGenres.AddRange(seriesGenres);
            context.SaveChanges();

            // Seed Ratings
            var ratings = new List<Rating>
            {
                new Rating
                {
                    UserId = users[1].Id,
                    MovieId = movies[0].Id,
                    Score = 9,
                    Comment = "Mind-blowing movie!",
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new Rating
                {
                    UserId = users[1].Id,
                    MovieId = movies[1].Id,
                    Score = 10,
                    Comment = "Masterpiece!",
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new Rating
                {
                    UserId = users[2].Id,
                    SeriesId = series[0].Id,
                    Score = 10,
                    Comment = "Best series ever!",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Rating
                {
                    UserId = users[2].Id,
                    SeriesId = series[2].Id,
                    Score = 8,
                    Comment = "Great nostalgic vibes!",
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                }
            };
            context.Ratings.AddRange(ratings);
            context.SaveChanges();

            // Seed ToWatch
            var toWatchList = new List<ToWatchList>
            {
                new ToWatchList
                {
                    UserId = users[1].Id,
                    MovieId = movies[2].Id,
                    AddedAt = DateTime.UtcNow.AddDays(-7)
                },
                new ToWatchList
                {
                    UserId = users[1].Id,
                    SeriesId = series[1].Id,
                    AddedAt = DateTime.UtcNow.AddDays(-6)
                },
                new ToWatchList
                {
                    UserId = users[2].Id,
                    MovieId = movies[4].Id,
                    AddedAt = DateTime.UtcNow.AddDays(-4)
                },
                new ToWatchList
                {
                    UserId = users[2].Id,
                    SeriesId = series[3].Id,
                    AddedAt = DateTime.UtcNow.AddDays(-2)
                }
            };
            context.ToWatchList.AddRange(toWatchList);
            context.SaveChanges();

            Console.WriteLine("Database seeding completed successfully!");
        }
    }
}
