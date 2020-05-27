using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartServer.Ef.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmartTemperatureClients",
                columns: table => new
                {
                    ChipId = table.Column<string>(nullable: false),
                    LastDataUpdate = table.Column<DateTime>(nullable: false),
                    Temperature = table.Column<double>(nullable: false),
                    Humidity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartTemperatureClients", x => x.ChipId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmartTemperatureClients");
        }
    }
}
