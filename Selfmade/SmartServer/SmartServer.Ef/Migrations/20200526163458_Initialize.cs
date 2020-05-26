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
                    DbId = table.Column<Guid>(nullable: false),
                    Discovered = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartTemperatureClients", x => x.DbId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmartTemperatureClients");
        }
    }
}
