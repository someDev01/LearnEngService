using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deleteLearnContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YoutubeVideos_LearningContents_LearningContentId",
                table: "YoutubeVideos");

            migrationBuilder.DropTable(
                name: "LearningContents");

            migrationBuilder.DropIndex(
                name: "IX_YoutubeVideos_LearningContentId",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "LearningContentId",
                table: "YoutubeVideos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LearningContentId",
                table: "YoutubeVideos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "LearningContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PosterKey = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningContents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeVideos_LearningContentId",
                table: "YoutubeVideos",
                column: "LearningContentId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningContents_Title",
                table: "LearningContents",
                column: "Title",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_YoutubeVideos_LearningContents_LearningContentId",
                table: "YoutubeVideos",
                column: "LearningContentId",
                principalTable: "LearningContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
