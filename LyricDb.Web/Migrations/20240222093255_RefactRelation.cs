using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LyricDb.Web.Migrations
{
    /// <inheritdoc />
    public partial class RefactRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lyrics_Songs_SongId",
                table: "Lyrics");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Lyrics_CurrentLyricId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_CurrentLyricId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "CurrentLyricId",
                table: "Songs");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentLyric",
                table: "Songs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "SongId",
                table: "Lyrics",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lyrics_Songs_SongId",
                table: "Lyrics",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lyrics_Songs_SongId",
                table: "Lyrics");

            migrationBuilder.DropColumn(
                name: "CurrentLyric",
                table: "Songs");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentLyricId",
                table: "Songs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SongId",
                table: "Lyrics",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_CurrentLyricId",
                table: "Songs",
                column: "CurrentLyricId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lyrics_Songs_SongId",
                table: "Lyrics",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Lyrics_CurrentLyricId",
                table: "Songs",
                column: "CurrentLyricId",
                principalTable: "Lyrics",
                principalColumn: "Id");
        }
    }
}
