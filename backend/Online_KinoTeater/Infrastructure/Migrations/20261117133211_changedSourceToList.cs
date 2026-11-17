using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedSourceToList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationContextHours",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "DurationContextMinutes",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "DurationContextSeconds",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Source_Context",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Source_YoutubeId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Source_YoutubeVideoId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "YoutubeVideoTitle",
                table: "Notes");

            migrationBuilder.CreateTable(
                name: "NoteSources",
                columns: table => new
                {
                    NoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    YoutubeVideoId = table.Column<Guid>(type: "uuid", nullable: false),
                    YoutubeId = table.Column<string>(type: "text", nullable: false),
                    YoutubeVideoTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Context = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DurationContextHours = table.Column<int>(type: "integer", nullable: false),
                    DurationContextMinutes = table.Column<int>(type: "integer", nullable: false),
                    DurationContextSeconds = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteSources", x => new { x.NoteId, x.Id });
                    table.ForeignKey(
                        name: "FK_NoteSources_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteSources");

            migrationBuilder.AddColumn<int>(
                name: "DurationContextHours",
                table: "Notes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationContextMinutes",
                table: "Notes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationContextSeconds",
                table: "Notes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source_Context",
                table: "Notes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source_YoutubeId",
                table: "Notes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Source_YoutubeVideoId",
                table: "Notes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YoutubeVideoTitle",
                table: "Notes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
