namespace AdsPortal.Persistence.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ContentTypeInMediaItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "MediaItems",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "MediaItems");
        }
    }
}
