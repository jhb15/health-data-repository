using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthDataRepository.Migrations
{
    public partial class ChangeSourceToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "Activity",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Source",
                table: "Activity",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
