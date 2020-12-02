namespace AdsPortal.Persistence.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ExternalAuthorsInsteadOfAuthorsTotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAuthors",
                table: "Publications");

            migrationBuilder.AddColumn<int>(
                name: "ExternalAuthors",
                table: "Publications",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalAuthors",
                table: "Publications");

            migrationBuilder.AddColumn<int>(
                name: "TotalAuthors",
                table: "Publications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
