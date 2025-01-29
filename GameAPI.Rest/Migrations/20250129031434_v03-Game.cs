using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameAPI.Rest.Migrations
{
    /// <inheritdoc />
    public partial class v03Game : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatform_Game_GamesId",
                table: "GamePlatform");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatform_Platform_PlatformsId",
                table: "GamePlatform");

            migrationBuilder.RenameColumn(
                name: "PlatformsId",
                table: "GamePlatform",
                newName: "PlatformId");

            migrationBuilder.RenameColumn(
                name: "GamesId",
                table: "GamePlatform",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlatform_PlatformsId",
                table: "GamePlatform",
                newName: "IX_GamePlatform_PlatformId");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlatform_Game_GameId",
                table: "GamePlatform",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlatform_Platform_PlatformId",
                table: "GamePlatform",
                column: "PlatformId",
                principalTable: "Platform",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatform_Game_GameId",
                table: "GamePlatform");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatform_Platform_PlatformId",
                table: "GamePlatform");

            migrationBuilder.RenameColumn(
                name: "PlatformId",
                table: "GamePlatform",
                newName: "PlatformsId");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "GamePlatform",
                newName: "GamesId");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlatform_PlatformId",
                table: "GamePlatform",
                newName: "IX_GamePlatform_PlatformsId");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlatform_Game_GamesId",
                table: "GamePlatform",
                column: "GamesId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlatform_Platform_PlatformsId",
                table: "GamePlatform",
                column: "PlatformsId",
                principalTable: "Platform",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
