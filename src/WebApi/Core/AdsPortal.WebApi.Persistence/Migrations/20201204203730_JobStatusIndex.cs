namespace AdsPortal.WebApi.Persistence.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class JobStatusIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Status",
                table: "Jobs",
                column: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Jobs_Status",
                table: "Jobs");
        }
    }
}
