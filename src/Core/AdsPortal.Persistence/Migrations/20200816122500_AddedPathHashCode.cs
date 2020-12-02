namespace AdsPortal.Persistence.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedPathHashCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PathHashCode",
                table: "MediaItems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_PathHashCode",
                table: "MediaItems",
                column: "PathHashCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MediaItems_PathHashCode",
                table: "MediaItems");

            migrationBuilder.DropColumn(
                name: "PathHashCode",
                table: "MediaItems");
        }
    }
}
