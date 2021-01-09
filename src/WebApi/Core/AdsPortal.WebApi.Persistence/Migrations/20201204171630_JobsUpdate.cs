namespace AdsPortal.WebApi.Persistence.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class JobsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OperationResult",
                table: "Jobs",
                newName: "Exception");

            migrationBuilder.RenameColumn(
                name: "OperationArguments",
                table: "Jobs",
                newName: "Arguments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Exception",
                table: "Jobs",
                newName: "OperationResult");

            migrationBuilder.RenameColumn(
                name: "Arguments",
                table: "Jobs",
                newName: "OperationArguments");
        }
    }
}
