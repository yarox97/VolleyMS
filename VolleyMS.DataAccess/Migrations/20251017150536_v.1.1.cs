using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationModel_Users_senderId",
                table: "NotificationModel");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationModelUserModel_NotificationModel_RecieverNotifi~",
                table: "NotificationModelUserModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationModel",
                table: "NotificationModel");

            migrationBuilder.RenameTable(
                name: "NotificationModel",
                newName: "Notifications");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationModel_senderId",
                table: "Notifications",
                newName: "IX_Notifications_senderId");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Notifications",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationModelUserModel_Notifications_RecieverNotificati~",
                table: "NotificationModelUserModel",
                column: "RecieverNotificationsModelsId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_senderId",
                table: "Notifications",
                column: "senderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationModelUserModel_Notifications_RecieverNotificati~",
                table: "NotificationModelUserModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_senderId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "NotificationModel");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_senderId",
                table: "NotificationModel",
                newName: "IX_NotificationModel_senderId");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "NotificationModel",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationModel",
                table: "NotificationModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationModel_Users_senderId",
                table: "NotificationModel",
                column: "senderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationModelUserModel_NotificationModel_RecieverNotifi~",
                table: "NotificationModelUserModel",
                column: "RecieverNotificationsModelsId",
                principalTable: "NotificationModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
