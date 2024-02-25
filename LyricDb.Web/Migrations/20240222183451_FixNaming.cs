using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LyricDb.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Songs",
                newName: "CreateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "Songs",
                newName: "CreateDate");
        }
    }
}
