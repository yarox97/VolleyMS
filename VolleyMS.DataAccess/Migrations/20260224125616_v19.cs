using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class v19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlockReachCm",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DominantHand",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HeightCm",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpikeReachCm",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WeightKg",
                table: "Users",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockReachCm",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DominantHand",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HeightCm",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SpikeReachCm",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WeightKg",
                table: "Users");
        }
    }
}
