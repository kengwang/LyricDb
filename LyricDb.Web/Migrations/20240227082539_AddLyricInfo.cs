using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LyricDb.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddLyricInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Lyrics",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Proofreader",
                table: "Lyrics",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timeline",
                table: "Lyrics",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Translator",
                table: "Lyrics",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Transliterator",
                table: "Lyrics",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Lyrics");

            migrationBuilder.DropColumn(
                name: "Proofreader",
                table: "Lyrics");

            migrationBuilder.DropColumn(
                name: "Timeline",
                table: "Lyrics");

            migrationBuilder.DropColumn(
                name: "Translator",
                table: "Lyrics");

            migrationBuilder.DropColumn(
                name: "Transliterator",
                table: "Lyrics");
        }
    }
}
