using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRental.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRentalHeaderDetailModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalHeaderDetails_RentalHeaders_RentalHeaderDetailId",
                table: "RentalHeaderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentalHeaderDetails",
                table: "RentalHeaderDetails");

            migrationBuilder.DropColumn(
                name: "RentalHeaderDetailId",
                table: "RentalHeaderDetails");

            migrationBuilder.AddColumn<int>(
                name: "RentalHeaderDetailId",
                table: "RentalHeaderDetails",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentalHeaderDetails",
                table: "RentalHeaderDetails",
                column: "RentalHeaderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalHeaderDetails_RentalHeaders_RentalHeaderId",
                table: "RentalHeaderDetails",
                column: "RentalHeaderId",
                principalTable: "RentalHeaders",
                principalColumn: "RentalHeaderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalHeaderDetails_RentalHeaders_RentalHeaderId",
                table: "RentalHeaderDetails");

            migrationBuilder.DropIndex(
                name: "IX_RentalHeaderDetails_RentalHeaderId",
                table: "RentalHeaderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "RentalHeaderDetailId",
                table: "RentalHeaderDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalHeaderDetails_RentalHeaders_RentalHeaderDetailId",
                table: "RentalHeaderDetails",
                column: "RentalHeaderDetailId",
                principalTable: "RentalHeaders",
                principalColumn: "RentalHeaderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
