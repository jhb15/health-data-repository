using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthDataRepository.Migrations
{
    public partial class FixMetresTravelledColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetersTravelled",
                table: "Activity");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Activity",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<double>(
                name: "MetresElevationGained",
                table: "Activity",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<double>(
                name: "MetresTravelled",
                table: "Activity",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetresTravelled",
                table: "Activity");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Activity",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<float>(
                name: "MetresElevationGained",
                table: "Activity",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<float>(
                name: "MetersTravelled",
                table: "Activity",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
