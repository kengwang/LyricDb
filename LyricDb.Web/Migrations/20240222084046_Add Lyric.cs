using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LyricDb.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddLyric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lyrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubmitterId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Reviewed = table.Column<bool>(type: "boolean", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lyrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lyrics_AspNetUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lyrics_AspNetUsers_SubmitterId",
                        column: x => x.SubmitterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Artists = table.Column<string>(type: "text", nullable: false),
                    Album = table.Column<string>(type: "text", nullable: false),
                    SubmitterId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentLyricId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Songs_AspNetUsers_SubmitterId",
                        column: x => x.SubmitterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Songs_Lyrics_CurrentLyricId",
                        column: x => x.CurrentLyricId,
                        principalTable: "Lyrics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lyrics_ReviewerId",
                table: "Lyrics",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Lyrics_SongId",
                table: "Lyrics",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Lyrics_SubmitterId",
                table: "Lyrics",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_CurrentLyricId",
                table: "Songs",
                column: "CurrentLyricId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_SubmitterId",
                table: "Songs",
                column: "SubmitterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lyrics_Songs_SongId",
                table: "Lyrics",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lyrics_Songs_SongId",
                table: "Lyrics");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Lyrics");
        }
    }
}
