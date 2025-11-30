using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequiredClubMemberRole",
                table: "Notifications",
                newName: "RequiredClubMemberRoles");

            migrationBuilder.AddColumn<string>(
                name: "Payload",
                table: "Notifications",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payload",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "RequiredClubMemberRoles",
                table: "Notifications",
                newName: "RequiredClubMemberRole");
        }
    }
}
