using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class dbV10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    notificationType = table.Column<int>(type: "integer", nullable: false),
                    isChecked = table.Column<bool>(type: "boolean", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    LinkedURL = table.Column<string>(type: "text", nullable: true),
                    senderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationModel_Users_senderId",
                        column: x => x.senderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationModelUserModel",
                columns: table => new
                {
                    ReceiversId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecieverNotificationsModelsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationModelUserModel", x => new { x.ReceiversId, x.RecieverNotificationsModelsId });
                    table.ForeignKey(
                        name: "FK_NotificationModelUserModel_NotificationModel_RecieverNotifi~",
                        column: x => x.RecieverNotificationsModelsId,
                        principalTable: "NotificationModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationModelUserModel_Users_ReceiversId",
                        column: x => x.ReceiversId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationModel_senderId",
                table: "NotificationModel",
                column: "senderId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationModelUserModel_RecieverNotificationsModelsId",
                table: "NotificationModelUserModel",
                column: "RecieverNotificationsModelsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationModelUserModel");

            migrationBuilder.DropTable(
                name: "NotificationModel");
        }
    }
}
