using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignerId",
                table: "Contracts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "VolleyBoxUrl",
                table: "Clubs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_AssignerId",
                table: "Contracts",
                column: "AssignerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Users_AssignerId",
                table: "Contracts",
                column: "AssignerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Users_AssignerId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_AssignerId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "AssignerId",
                table: "Contracts");

            migrationBuilder.AlterColumn<string>(
                name: "VolleyBoxUrl",
                table: "Clubs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
