namespace AdsPortal.WebApi.Persistence.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedMediaItemTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Alt",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a76b1435-1fe5-4cec-b1fc-083190fa7ec5"),
                columns: new[] { "CreatedOn", "LastSavedOn" },
                values: new object[] { new DateTime(2021, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "MediaItems");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Alt",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a76b1435-1fe5-4cec-b1fc-083190fa7ec5"),
                columns: new[] { "CreatedOn", "LastSavedOn" },
                values: new object[] { new DateTime(2021, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc) });
        }
    }
}
