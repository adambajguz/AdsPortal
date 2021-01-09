namespace AdsPortal.WebApi.Persistence.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class JobsIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobNo",
                table: "Jobs",
                column: "JobNo");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Priority",
                table: "Jobs",
                column: "Priority");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobNo",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_Priority",
                table: "Jobs");
        }
    }
}
