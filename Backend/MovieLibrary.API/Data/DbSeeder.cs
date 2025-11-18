using MovieLibrary.API.Models;

namespace MovieLibrary.API.Data
{
    public static class DbSeeder
    {
        public static void SeedData(MovieLibraryDbContext context)
        {
            // Check if data already exists
            if (context.Users.Any() || context.Genres.Any() || context.Titles.Any())
            {
                return; // Database has been seeded
            }

            // Seed all data in correct order
            var genres = SeedGenres(context);
            var users = SeedUsers(context);
            var titles = SeedTitles(context);
            SeedTitleGenres(context, titles, genres);
            SeedRatings(context, users, titles);
            SeedMyList(context, users, titles);

            Console.WriteLine("Database seeding completed successfully!");
        }

        private static List<Genre> SeedGenres(MovieLibraryDbContext context)
        {
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

            return genres;
        }

        private static List<User> SeedUsers(MovieLibraryDbContext context)
        {
            // Password for all users is: "password123"
            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    Email = "admin@movielibrary.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Username = "john_doe",
                    Email = "john.doe@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "User",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Username = "jane_smith",
                    Email = "jane@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    Role = "User",
                    CreatedAt = DateTime.UtcNow
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            return users;
        }

        private static List<Title> SeedTitles(MovieLibraryDbContext context)
        {
            var titles = new List<Title>
            {
                // Movies
                new Title
                {
                    Name = "The Matrix",
                    Description = "A computer hacker learns about the true nature of his reality.",
                    ReleaseYear = 1999,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/p96dm7sCMn4VYAStA6siNz30G1r.jpg"
                },
                new Title
                {
                    Name = "Inception",
                    Description = "A thief who steals corporate secrets through dream-sharing technology.",
                    ReleaseYear = 2010,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/ljsZTbVsrQSqZgWeep2B1QiDKuh.jpg"
                },
                new Title
                {
                    Name = "The Godfather",
                    Description = "The aging patriarch of an organized crime dynasty transfers control.",
                    ReleaseYear = 1972,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/3bhkrj58Vtu7enYsRolD1fZdja1.jpg"
                },
                new Title
                {
                    Name = "Pulp Fiction",
                    Description = "The lives of two mob hitmen, a boxer, and a pair of diner bandits intertwine.",
                    ReleaseYear = 1994,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/vQWk5YBFWF4bZaofAbv0tShwBvQ.jpg"
                },
                new Title
                {
                    Name = "Interstellar",
                    Description = "A team of explorers travel through a wormhole in space.",
                    ReleaseYear = 2014,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg"
                },
                // Series
                new Title
                {
                    Name = "Breaking Bad",
                    Description = "A high school chemistry teacher turned meth producer.",
                    ReleaseYear = 2008,
                    Type = TitleType.Series,
                    SeasonsCount = 5,
                    EpisodesCount = 62,
                    ImageUrl = "https://image.tmdb.org/t/p/original/ztkUQFLlC19CCMYHW9o1zWhJRNq.jpg"
                },
                new Title
                {
                    Name = "Game of Thrones",
                    Description = "Nine noble families fight for control over the lands of Westeros.",
                    ReleaseYear = 2011,
                    Type = TitleType.Series,
                    SeasonsCount = 8,
                    EpisodesCount = 73,
                    ImageUrl = "https://image.tmdb.org/t/p/original/1XS1oqL89opfnbLl8WnZY1O1uJx.jpg"
                },
                new Title
                {
                    Name = "Stranger Things",
                    Description = "When a young boy disappears, his mother and friends uncover supernatural mysteries.",
                    ReleaseYear = 2016,
                    Type = TitleType.Series,
                    SeasonsCount = 4,
                    EpisodesCount = 42,
                    ImageUrl = "https://image.tmdb.org/t/p/original/uOOtwVbSr4QDjAGIifLDwpb2Pdl.jpg"
                },
                new Title
                {
                    Name = "The Office",
                    Description = "A mockumentary on a group of typical office workers.",
                    ReleaseYear = 2005,
                    Type = TitleType.Series,
                    SeasonsCount = 9,
                    EpisodesCount = 201,
                    ImageUrl = "https://image.tmdb.org/t/p/original/7DJKHzAi83BmQrWLrYYOqcoKfhR.jpg"
                }
            };
            context.Titles.AddRange(titles);
            context.SaveChanges();

            return titles;
        }

        private static void SeedTitleGenres(MovieLibraryDbContext context, List<Title> titles, List<Genre> genres)
        {
            var titleGenres = new List<TitleGenre>
            {
                // The Matrix - Action, Sci-Fi
                new TitleGenre { TitleId = titles[0].Id, GenreId = genres[0].Id },
                new TitleGenre { TitleId = titles[0].Id, GenreId = genres[4].Id },

                // Inception - Action, Sci-Fi, Thriller
                new TitleGenre { TitleId = titles[1].Id, GenreId = genres[0].Id },
                new TitleGenre { TitleId = titles[1].Id, GenreId = genres[4].Id },
                new TitleGenre { TitleId = titles[1].Id, GenreId = genres[6].Id },

                // The Godfather - Drama
                new TitleGenre { TitleId = titles[2].Id, GenreId = genres[2].Id },

                // Pulp Fiction - Drama, Thriller
                new TitleGenre { TitleId = titles[3].Id, GenreId = genres[2].Id },
                new TitleGenre { TitleId = titles[3].Id, GenreId = genres[6].Id },

                // Interstellar - Sci-Fi, Drama
                new TitleGenre { TitleId = titles[4].Id, GenreId = genres[4].Id },
                new TitleGenre { TitleId = titles[4].Id, GenreId = genres[2].Id },

                // Breaking Bad - Drama, Thriller
                new TitleGenre { TitleId = titles[5].Id, GenreId = genres[2].Id },
                new TitleGenre { TitleId = titles[5].Id, GenreId = genres[6].Id },

                // Game of Thrones - Drama, Fantasy
                new TitleGenre { TitleId = titles[6].Id, GenreId = genres[2].Id },
                new TitleGenre { TitleId = titles[6].Id, GenreId = genres[9].Id },

                // Stranger Things - Horror, Sci-Fi, Thriller
                new TitleGenre { TitleId = titles[7].Id, GenreId = genres[3].Id },
                new TitleGenre { TitleId = titles[7].Id, GenreId = genres[4].Id },
                new TitleGenre { TitleId = titles[7].Id, GenreId = genres[6].Id },

                // The Office - Comedy
                new TitleGenre { TitleId = titles[8].Id, GenreId = genres[1].Id }
            };
            context.TitleGenres.AddRange(titleGenres);
            context.SaveChanges();
        }

        private static void SeedRatings(MovieLibraryDbContext context, List<User> users, List<Title> titles)
        {
            var ratings = new List<Rating>
            {
                new Rating
                {
                    UserId = users[1].Id,
                    TitleId = titles[0].Id, // The Matrix
                    Score = 9,
                    Comment = "Mind-blowing movie!",
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new Rating
                {
                    UserId = users[1].Id,
                    TitleId = titles[1].Id, // Inception
                    Score = 10,
                    Comment = "Masterpiece!",
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new Rating
                {
                    UserId = users[2].Id,
                    TitleId = titles[5].Id, // Breaking Bad
                    Score = 10,
                    Comment = "Best series ever!",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Rating
                {
                    UserId = users[2].Id,
                    TitleId = titles[7].Id, // Stranger Things
                    Score = 8,
                    Comment = "Great nostalgic vibes!",
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                }
            };
            context.Ratings.AddRange(ratings);
            context.SaveChanges();
        }

        private static void SeedMyList(MovieLibraryDbContext context, List<User> users, List<Title> titles)
        {
            var myList = new List<MyList>
            {
                new MyList
                {
                    UserId = users[1].Id,
                    TitleId = titles[2].Id, // The Godfather
                    Status = WatchStatus.PlanToWatch,
                    AddedAt = DateTime.UtcNow.AddDays(-7)
                },
                new MyList
                {
                    UserId = users[1].Id,
                    TitleId = titles[6].Id, // Game of Thrones
                    Status = WatchStatus.Watching,
                    AddedAt = DateTime.UtcNow.AddDays(-6),
                    StatusUpdatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new MyList
                {
                    UserId = users[1].Id,
                    TitleId = titles[0].Id, // The Matrix
                    Status = WatchStatus.Completed,
                    AddedAt = DateTime.UtcNow.AddDays(-15),
                    StatusUpdatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new MyList
                {
                    UserId = users[2].Id,
                    TitleId = titles[4].Id, // Interstellar
                    Status = WatchStatus.PlanToWatch,
                    AddedAt = DateTime.UtcNow.AddDays(-4)
                },
                new MyList
                {
                    UserId = users[2].Id,
                    TitleId = titles[8].Id, // The Office
                    Status = WatchStatus.OnHold,
                    AddedAt = DateTime.UtcNow.AddDays(-20),
                    StatusUpdatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new MyList
                {
                    UserId = users[2].Id,
                    TitleId = titles[5].Id, // Breaking Bad
                    Status = WatchStatus.Completed,
                    AddedAt = DateTime.UtcNow.AddDays(-30),
                    StatusUpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new MyList
                {
                    UserId = users[1].Id,
                    TitleId = titles[3].Id, // Pulp Fiction
                    Status = WatchStatus.Dropped,
                    AddedAt = DateTime.UtcNow.AddDays(-12),
                    StatusUpdatedAt = DateTime.UtcNow.AddDays(-9)
                }
            };
            context.MyLists.AddRange(myList);
            context.SaveChanges();
        }
    }
}
