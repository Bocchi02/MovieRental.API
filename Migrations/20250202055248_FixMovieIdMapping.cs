using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRental.API.Migrations
{
    /// <inheritdoc />
    public partial class FixMovieIdMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalHeaderDetails_Movies_MovieId",
                table: "RentalHeaderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalHeaderDetails_Movies_MovieId",
                table: "RentalHeaderDetails",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalHeaderDetails_Movies_MovieId",
                table: "RentalHeaderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalHeaderDetails_Movies_MovieId",
                table: "RentalHeaderDetails",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
