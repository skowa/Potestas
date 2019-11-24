using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace Potestas.CodeFirst.Migrations
{
    public partial class CreatePotestasEF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlashObservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObservationPoint = table.Column<Point>(type: "geometry", nullable: true),
                    Intensity = table.Column<double>(nullable: false),
                    DurationMs = table.Column<int>(nullable: false),
                    ObservationTime = table.Column<DateTime>(nullable: false),
                    EstimatedValue = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashObservations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlashObservations");
        }
    }
}
