using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdsPortal.WebApi.Persistence.Migrations
{
    public partial class DefaultUserDateFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a76b1435-1fe5-4cec-b1fc-083190fa7ec5"),
                columns: new[] { "CreatedOn", "LastSavedOn" },
                values: new object[] { new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a76b1435-1fe5-4cec-b1fc-083190fa7ec5"),
                columns: new[] { "CreatedOn", "LastSavedOn" },
                values: new object[] { new DateTime(2021, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc) });
        }
    }
}
