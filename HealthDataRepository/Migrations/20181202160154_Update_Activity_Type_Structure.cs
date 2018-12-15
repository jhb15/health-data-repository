using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthDataRepository.Migrations
{
    public partial class Update_Activity_Type_Structure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SourceActivity");

            migrationBuilder.DropTable(
                name: "ActivityTypeSource");

            migrationBuilder.CreateTable(
                name: "ActivityMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Source = table.Column<int>(nullable: false),
                    MappingKey = table.Column<string>(nullable: false),
                    ActivityTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityMapping_ActivityType_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "ActivityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityMapping_ActivityTypeId",
                table: "ActivityMapping",
                column: "ActivityTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityMapping");

            migrationBuilder.CreateTable(
                name: "ActivityTypeSource",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityTypeId = table.Column<int>(nullable: true),
                    Source = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTypeSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityTypeSource_ActivityType_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "ActivityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SourceActivity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityTypeSourceId = table.Column<int>(nullable: true),
                    ServiceActivityId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceActivity_ActivityTypeSource_ActivityTypeSourceId",
                        column: x => x.ActivityTypeSourceId,
                        principalTable: "ActivityTypeSource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTypeSource_ActivityTypeId",
                table: "ActivityTypeSource",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceActivity_ActivityTypeSourceId",
                table: "SourceActivity",
                column: "ActivityTypeSourceId");
        }
    }
}
