using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LyricDb.Web.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToLyricStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Lyrics");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Lyrics",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Lyrics");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Lyrics",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
