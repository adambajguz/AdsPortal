namespace AdsPortal.Persistence.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdatedJournalEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VirtualPath",
                table: "MediaItems",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "MediaItems",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Disciplines",
                table: "Journals",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EISSN",
                table: "Journals",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ISSN",
                table: "Journals",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disciplines",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "EISSN",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "ISSN",
                table: "Journals");

            migrationBuilder.AlterColumn<string>(
                name: "VirtualPath",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "MediaItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
