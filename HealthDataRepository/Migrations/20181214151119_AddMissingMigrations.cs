using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthDataRepository.Migrations
{
    public partial class AddMissingMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityMapping_ActivityType_ActivityTypeId",
                table: "ActivityMapping");

            migrationBuilder.AlterColumn<int>(
                name: "ActivityTypeId",
                table: "ActivityMapping",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityMapping_ActivityType_ActivityTypeId",
                table: "ActivityMapping",
                column: "ActivityTypeId",
                principalTable: "ActivityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityMapping_ActivityType_ActivityTypeId",
                table: "ActivityMapping");

            migrationBuilder.AlterColumn<int>(
                name: "ActivityTypeId",
                table: "ActivityMapping",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityMapping_ActivityType_ActivityTypeId",
                table: "ActivityMapping",
                column: "ActivityTypeId",
                principalTable: "ActivityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
