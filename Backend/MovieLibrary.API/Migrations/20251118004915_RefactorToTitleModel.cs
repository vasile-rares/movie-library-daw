using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieLibrary.API.Migrations
{
    /// <inheritdoc />
    public partial class RefactorToTitleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Movies_MovieId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Series_SeriesId",
                table: "Ratings");

            migrationBuilder.DropTable(
                name: "MovieGenres");

            migrationBuilder.DropTable(
                name: "SeriesGenres");

            migrationBuilder.DropTable(
                name: "ToWatch");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_MovieId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_SeriesId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "TitleId",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Titles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReleaseYear = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    SeasonsCount = table.Column<int>(type: "int", nullable: true),
                    EpisodesCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TitleId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyList_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MyList_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TitleGenres",
                columns: table => new
                {
                    TitleId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleGenres", x => new { x.TitleId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_TitleGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TitleGenres_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_TitleId",
                table: "Ratings",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_MyList_TitleId",
                table: "MyList",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_MyList_UserId_TitleId",
                table: "MyList",
                columns: new[] { "UserId", "TitleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TitleGenres_GenreId",
                table: "TitleGenres",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Titles_TitleId",
                table: "Ratings",
                column: "TitleId",
                principalTable: "Titles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Titles_TitleId",
                table: "Ratings");

            migrationBuilder.DropTable(
                name: "MyList");

            migrationBuilder.DropTable(
                name: "TitleGenres");

            migrationBuilder.DropTable(
                name: "Titles");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_TitleId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "TitleId",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeriesId",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReleaseYear = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EpisodesCount = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReleaseYear = table.Column<int>(type: "int", nullable: true),
                    SeasonsCount = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieGenres",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenres", x => new { x.MovieId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_MovieGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieGenres_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriesGenres",
                columns: table => new
                {
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesGenres", x => new { x.SeriesId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_SeriesGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesGenres_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToWatch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: true),
                    SeriesId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToWatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToWatch_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ToWatch_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ToWatch_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_MovieId",
                table: "Ratings",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_SeriesId",
                table: "Ratings",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_GenreId",
                table: "MovieGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesGenres_GenreId",
                table: "SeriesGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_ToWatch_MovieId",
                table: "ToWatch",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_ToWatch_SeriesId",
                table: "ToWatch",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ToWatch_UserId",
                table: "ToWatch",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Movies_MovieId",
                table: "Ratings",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Series_SeriesId",
                table: "Ratings",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id");
        }
    }
}
