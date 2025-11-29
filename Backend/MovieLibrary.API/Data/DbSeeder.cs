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
                    Nickname = "admin",
                    Email = "admin@movielibrary.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    ProfilePictureUrl = "https://api.dicebear.com/7.x/avataaars/svg?seed=admin",
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Nickname = "john_doe",
                    Email = "john.doe@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    ProfilePictureUrl = "https://api.dicebear.com/7.x/avataaars/svg?seed=john",
                    Role = "User",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Nickname = "jane_smith",
                    Email = "jane@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    ProfilePictureUrl = "https://api.dicebear.com/7.x/avataaars/svg?seed=jane",
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
                new Title
                {
                    Name = "The Dark Knight",
                    Description = "Batman faces the Joker, a criminal mastermind who wants to plunge Gotham into anarchy.",
                    ReleaseYear = 2008,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/qJ2tW6WMUDux911r6m7haRef0WH.jpg"
                },
                new Title
                {
                    Name = "Fight Club",
                    Description = "An insomniac office worker forms an underground fight club.",
                    ReleaseYear = 1999,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/pB8BM7pdSp6B6Ih7QZ4DrQ3PmJK.jpg"
                },
                new Title
                {
                    Name = "Forrest Gump",
                    Description = "The presidencies of Kennedy and Johnson unfold through the perspective of an Alabama man.",
                    ReleaseYear = 1994,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/arw2vcBveWOVZr6pxd9XTd1TdQa.jpg"
                },
                new Title
                {
                    Name = "The Shawshank Redemption",
                    Description = "Two imprisoned men bond over a number of years, finding solace and redemption.",
                    ReleaseYear = 1994,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/9cqNxx0GxF0bflZmeSMuL5tnGzr.jpg"
                },
                new Title
                {
                    Name = "Gladiator",
                    Description = "A former Roman General sets out to exact vengeance against the corrupt emperor.",
                    ReleaseYear = 2000,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/ty8TGRuvJLPUmAR1H1nRIsgwvim.jpg"
                },
                new Title
                {
                    Name = "The Prestige",
                    Description = "Two stage magicians engage in competitive one-upmanship in an attempt to create the ultimate illusion.",
                    ReleaseYear = 2006,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/tRNlZbgNCNOpLpbPEz5L8G8A0JN.jpg"
                },
                new Title
                {
                    Name = "Parasite",
                    Description = "Greed and class discrimination threaten the newly formed symbiotic relationship between the wealthy Park family and the destitute Kim clan.",
                    ReleaseYear = 2019,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/7IiTTgloJzvGI1TAYymCfbfl3vT.jpg"
                },
                new Title
                {
                    Name = "Avatar",
                    Description = "A paraplegic Marine dispatched to the moon Pandora on a unique mission becomes torn between following orders and protecting an alien civilization.",
                    ReleaseYear = 2009,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/kyeqWdyUXW608qlYkRqosgbbJyK.jpg"
                },
                new Title
                {
                    Name = "Avengers: Endgame",
                    Description = "After the devastating events of Infinity War, the Avengers assemble once more to reverse Thanos' actions.",
                    ReleaseYear = 2019,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/ulzhLuWrPK07P1YkdWQLZnQh1JL.jpg"
                },
                new Title
                {
                    Name = "Joker",
                    Description = "In Gotham City, mentally troubled comedian Arthur Fleck is disregarded and mistreated by society.",
                    ReleaseYear = 2019,
                    Type = TitleType.Movie,
                    ImageUrl = "https://image.tmdb.org/t/p/original/udDclJoHjfjb8Ekgsd4FDteOkCU.jpg"
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
                },
                new Title
                {
                    Name = "The Witcher",
                    Description = "Geralt of Rivia, a mutated monster-hunter for hire, journeys toward his destiny in a turbulent world.",
                    ReleaseYear = 2019,
                    Type = TitleType.Series,
                    SeasonsCount = 3,
                    EpisodesCount = 24,
                    ImageUrl = "https://image.tmdb.org/t/p/original/7vjaCdMw15FEbXyLQTVa04URsPm.jpg"
                },
                new Title
                {
                    Name = "The Crown",
                    Description = "Follows the political rivalries and romance of Queen Elizabeth II's reign.",
                    ReleaseYear = 2016,
                    Type = TitleType.Series,
                    SeasonsCount = 6,
                    EpisodesCount = 60,
                    ImageUrl = "https://image.tmdb.org/t/p/original/1M876KPjulVwppEpldhdc8V4o68.jpg"
                },
                new Title
                {
                    Name = "The Mandalorian",
                    Description = "The travels of a lone bounty hunter in the outer reaches of the galaxy.",
                    ReleaseYear = 2019,
                    Type = TitleType.Series,
                    SeasonsCount = 3,
                    EpisodesCount = 24,
                    ImageUrl = "https://image.tmdb.org/t/p/original/eU1i6eHXlzMOlEq0ku1Rzq7Y4wA.jpg"
                },
                new Title
                {
                    Name = "Friends",
                    Description = "Follows the personal and professional lives of six friends living in Manhattan.",
                    ReleaseYear = 1994,
                    Type = TitleType.Series,
                    SeasonsCount = 10,
                    EpisodesCount = 236,
                    ImageUrl = "https://image.tmdb.org/t/p/original/2koX1xLkpTQM4IZebYvaxgJYSDy.jpg"
                },
                new Title
                {
                    Name = "The Last of Us",
                    Description = "After a global pandemic destroys civilization, a hardened survivor takes charge of a 14-year-old girl.",
                    ReleaseYear = 2023,
                    Type = TitleType.Series,
                    SeasonsCount = 1,
                    EpisodesCount = 9,
                    ImageUrl = "https://image.tmdb.org/t/p/original/uKvVjHNqB5VmOrdxqAt2F7J78ED.jpg"
                },
                new Title
                {
                    Name = "Peaky Blinders",
                    Description = "A gangster family epic set in 1900s England, centering on a gang who sew razor blades in the peaks of their caps.",
                    ReleaseYear = 2013,
                    Type = TitleType.Series,
                    SeasonsCount = 6,
                    EpisodesCount = 36,
                    ImageUrl = "https://image.tmdb.org/t/p/original/vUUqzWa2LnHIVqkaKVlVGkVcZIW.jpg"
                },
                new Title
                {
                    Name = "Wednesday",
                    Description = "Wednesday Addams' years as a student at Nevermore Academy.",
                    ReleaseYear = 2022,
                    Type = TitleType.Series,
                    SeasonsCount = 1,
                    EpisodesCount = 8,
                    ImageUrl = "https://image.tmdb.org/t/p/original/9PFonBhy4cQy7Jz20NpMygczOkv.jpg"
                },
                new Title
                {
                    Name = "Sherlock",
                    Description = "A modern update finds the famous sleuth and his doctor partner solving crime in 21st century London.",
                    ReleaseYear = 2010,
                    Type = TitleType.Series,
                    SeasonsCount = 4,
                    EpisodesCount = 13,
                    ImageUrl = "https://image.tmdb.org/t/p/original/7WTsnHkbA0FaG6R9twfFde0I9hl.jpg"
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
                // Movies
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

                // The Dark Knight - Action, Thriller
                new TitleGenre { TitleId = titles[5].Id, GenreId = genres[0].Id },
                new TitleGenre { TitleId = titles[5].Id, GenreId = genres[6].Id },

                // Fight Club - Drama, Thriller
                new TitleGenre { TitleId = titles[6].Id, GenreId = genres[2].Id },
                new TitleGenre { TitleId = titles[6].Id, GenreId = genres[6].Id },

                // Forrest Gump - Drama, Romance
                new TitleGenre { TitleId = titles[7].Id, GenreId = genres[2].Id },
                new TitleGenre { TitleId = titles[7].Id, GenreId = genres[5].Id },

                // The Shawshank Redemption - Drama
                new TitleGenre { TitleId = titles[8].Id, GenreId = genres[2].Id },

                // Gladiator - Action, Drama
                new TitleGenre { TitleId = titles[9].Id, GenreId = genres[0].Id },
                new TitleGenre { TitleId = titles[9].Id, GenreId = genres[2].Id },

                // The Prestige - Drama, Thriller
                new TitleGenre { TitleId = titles[10].Id, GenreId = genres[2].Id },
                new TitleGenre { TitleId = titles[10].Id, GenreId = genres[6].Id },

                // Parasite - Drama, Thriller
                new TitleGenre { TitleId = titles[11].Id, GenreId = genres[2].Id },
                new TitleGenre { TitleId = titles[11].Id, GenreId = genres[6].Id },

                // Avatar - Action, Sci-Fi, Fantasy
                new TitleGenre { TitleId = titles[12].Id, GenreId = genres[0].Id },
                new TitleGenre { TitleId = titles[12].Id, GenreId = genres[4].Id },
                new TitleGenre { TitleId = titles[12].Id, GenreId = genres[9].Id },

                // Avengers: Endgame - Action, Sci-Fi
                new TitleGenre { TitleId = titles[13].Id, GenreId = genres[0].Id },
                new TitleGenre { TitleId = titles[13].Id, GenreId = genres[4].Id },

                // Joker - Drama, Thriller
                new TitleGenre { TitleId = titles[14].Id, GenreId = genres[2].Id },
                new TitleGenre { TitleId = titles[14].Id, GenreId = genres[6].Id },

                // Series
                // Breaking Bad - Drama, Thriller
                new TitleGenre { TitleId = titles[15].Id, GenreId = genres[2].Id },
                new TitleGenre { TitleId = titles[15].Id, GenreId = genres[6].Id },

                // Game of Thrones - Drama, Fantasy
                new TitleGenre { TitleId = titles[16].Id, GenreId = genres[2].Id },
                new TitleGenre { TitleId = titles[16].Id, GenreId = genres[9].Id },

                // Stranger Things - Horror, Sci-Fi, Thriller
                new TitleGenre { TitleId = titles[17].Id, GenreId = genres[3].Id },
                new TitleGenre { TitleId = titles[17].Id, GenreId = genres[4].Id },
                new TitleGenre { TitleId = titles[17].Id, GenreId = genres[6].Id },

                // The Office - Comedy
                new TitleGenre { TitleId = titles[18].Id, GenreId = genres[1].Id },

                // The Witcher - Action, Fantasy
                new TitleGenre { TitleId = titles[19].Id, GenreId = genres[0].Id },
                new TitleGenre { TitleId = titles[19].Id, GenreId = genres[9].Id },

                // The Crown - Drama
                new TitleGenre { TitleId = titles[20].Id, GenreId = genres[2].Id },

                // The Mandalorian - Action, Sci-Fi
                new TitleGenre { TitleId = titles[21].Id, GenreId = genres[0].Id },
                new TitleGenre { TitleId = titles[21].Id, GenreId = genres[4].Id },

                // Friends - Comedy, Romance
                new TitleGenre { TitleId = titles[22].Id, GenreId = genres[1].Id },
                new TitleGenre { TitleId = titles[22].Id, GenreId = genres[5].Id },

                // The Last of Us - Drama, Horror, Thriller
                new TitleGenre { TitleId = titles[23].Id, GenreId = genres[2].Id },
                new TitleGenre { TitleId = titles[23].Id, GenreId = genres[3].Id },
                new TitleGenre { TitleId = titles[23].Id, GenreId = genres[6].Id },

                // Peaky Blinders - Drama, Thriller
                new TitleGenre { TitleId = titles[24].Id, GenreId = genres[2].Id },
                new TitleGenre { TitleId = titles[24].Id, GenreId = genres[6].Id },

                // Wednesday - Comedy, Horror, Fantasy
                new TitleGenre { TitleId = titles[25].Id, GenreId = genres[1].Id },
                new TitleGenre { TitleId = titles[25].Id, GenreId = genres[3].Id },
                new TitleGenre { TitleId = titles[25].Id, GenreId = genres[9].Id },

                // Sherlock - Drama, Thriller
                new TitleGenre { TitleId = titles[26].Id, GenreId = genres[2].Id },
                new TitleGenre { TitleId = titles[26].Id, GenreId = genres[6].Id }
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
