namespace AdsPortal.Persistence.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class VirtualDirectoryInsteadOfPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VirtualPath",
                table: "MediaItems");

            migrationBuilder.AddColumn<string>(
                name: "VirtualDirectory",
                table: "MediaItems",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VirtualDirectory",
                table: "MediaItems");

            migrationBuilder.AddColumn<string>(
                name: "VirtualPath",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
