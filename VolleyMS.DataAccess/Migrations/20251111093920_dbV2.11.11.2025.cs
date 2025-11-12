using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class dbV211112025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubEntityUserEntity");

            migrationBuilder.DropColumn(
                name: "notificationType",
                table: "Notifications");

            migrationBuilder.AddColumn<Guid>(
                name: "notificationTypeId",
                table: "Notifications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "NotificationTypeEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    notificationCategory = table.Column<int>(type: "integer", nullable: false),
                    requiredClubMemberRole = table.Column<int[]>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypeEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserClubs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClubId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClubMemberRole = table.Column<int[]>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClubs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClubs_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserClubs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_notificationTypeId",
                table: "Notifications",
                column: "notificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClubs_ClubId",
                table: "UserClubs",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClubs_UserId",
                table: "UserClubs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationTypeEntity_notificationTypeId",
                table: "Notifications",
                column: "notificationTypeId",
                principalTable: "NotificationTypeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationTypeEntity_notificationTypeId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "NotificationTypeEntity");

            migrationBuilder.DropTable(
                name: "UserClubs");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_notificationTypeId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "notificationTypeId",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "notificationType",
                table: "Notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClubEntityUserEntity",
                columns: table => new
                {
                    ClubsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubEntityUserEntity", x => new { x.ClubsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ClubEntityUserEntity_Clubs_ClubsId",
                        column: x => x.ClubsId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubEntityUserEntity_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubEntityUserEntity_UsersId",
                table: "ClubEntityUserEntity",
                column: "UsersId");
        }
    }
}
