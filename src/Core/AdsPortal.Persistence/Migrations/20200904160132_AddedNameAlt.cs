using Microsoft.EntityFrameworkCore.Migrations;

namespace AdsPortal.Persistence.Migrations
{
    public partial class AddedNameAlt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameAlt",
                table: "Journals",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAlt",
                table: "Journals");
        }
    }
}
