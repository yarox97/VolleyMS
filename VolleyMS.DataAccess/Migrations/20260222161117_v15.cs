using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class v15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Payslips_ContractId",
                table: "Payslips",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payslips_Contracts_ContractId",
                table: "Payslips",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payslips_Users_UserId",
                table: "Payslips",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payslips_Contracts_ContractId",
                table: "Payslips");

            migrationBuilder.DropForeignKey(
                name: "FK_Payslips_Users_UserId",
                table: "Payslips");

            migrationBuilder.DropIndex(
                name: "IX_Payslips_ContractId",
                table: "Payslips");
        }
    }
}
