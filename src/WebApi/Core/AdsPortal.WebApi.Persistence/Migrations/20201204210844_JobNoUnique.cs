namespace AdsPortal.WebApi.Persistence.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class JobNoUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobNo",
                table: "Jobs");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobNo",
                table: "Jobs",
                column: "JobNo",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobNo",
                table: "Jobs");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobNo",
                table: "Jobs",
                column: "JobNo");
        }
    }
}
