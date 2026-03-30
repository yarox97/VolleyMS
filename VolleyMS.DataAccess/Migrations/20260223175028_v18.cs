using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class v18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payslips_ContractId",
                table: "Payslips");

            migrationBuilder.DropIndex(
                name: "IX_Payslips_UserId_Month_Year",
                table: "Payslips");

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_ContractId_Month_Year",
                table: "Payslips",
                columns: new[] { "ContractId", "Month", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_UserId_Month_Year",
                table: "Payslips",
                columns: new[] { "UserId", "Month", "Year" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payslips_ContractId_Month_Year",
                table: "Payslips");

            migrationBuilder.DropIndex(
                name: "IX_Payslips_UserId_Month_Year",
                table: "Payslips");

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_ContractId",
                table: "Payslips",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_UserId_Month_Year",
                table: "Payslips",
                columns: new[] { "UserId", "Month", "Year" },
                unique: true);
        }
    }
}
