using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initFix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClubs_Clubs_ClubId1",
                table: "UserClubs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClubs_Users_UserId1",
                table: "UserClubs");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "UserClubs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ClubId1",
                table: "UserClubs",
                newName: "ClubId");

            migrationBuilder.RenameIndex(
                name: "IX_UserClubs_UserId1",
                table: "UserClubs",
                newName: "IX_UserClubs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserClubs_ClubId1",
                table: "UserClubs",
                newName: "IX_UserClubs_ClubId");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Clubs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackGroundURL",
                table: "Clubs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Clubs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Clubs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClubs_Clubs_ClubId",
                table: "UserClubs",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClubs_Users_UserId",
                table: "UserClubs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClubs_Clubs_ClubId",
                table: "UserClubs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClubs_Users_UserId",
                table: "UserClubs");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "BackGroundURL",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Clubs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserClubs",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "ClubId",
                table: "UserClubs",
                newName: "ClubId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserClubs_UserId",
                table: "UserClubs",
                newName: "IX_UserClubs_UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserClubs_ClubId",
                table: "UserClubs",
                newName: "IX_UserClubs_ClubId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClubs_Clubs_ClubId1",
                table: "UserClubs",
                column: "ClubId1",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClubs_Users_UserId1",
                table: "UserClubs",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
