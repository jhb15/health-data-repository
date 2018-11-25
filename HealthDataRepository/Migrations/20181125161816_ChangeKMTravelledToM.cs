using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthDataRepository.Migrations
{
    public partial class ChangeKMTravelledToM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KilometersTravelled",
                table: "Activity",
                newName: "MetersTravelled");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MetersTravelled",
                table: "Activity",
                newName: "KilometersTravelled");
        }
    }
}
