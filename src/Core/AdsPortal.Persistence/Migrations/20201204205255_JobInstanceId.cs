using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdsPortal.Persistence.Migrations
{
    public partial class JobInstanceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Instance",
                table: "Jobs",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instance",
                table: "Jobs");
        }
    }
}
