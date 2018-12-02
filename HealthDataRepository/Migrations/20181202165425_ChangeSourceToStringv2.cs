using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthDataRepository.Migrations
{
    public partial class ChangeSourceToStringv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "ActivityMapping",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Source",
                table: "ActivityMapping",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
