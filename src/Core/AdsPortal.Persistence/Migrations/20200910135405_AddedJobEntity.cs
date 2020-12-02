namespace AdsPortal.Persistence.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedJobEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    JobNo = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    StartedOn = table.Column<DateTime>(nullable: true),
                    FinishedOn = table.Column<DateTime>(nullable: true),
                    Status = table.Column<long>(nullable: false),
                    Operation = table.Column<string>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    PostponeTo = table.Column<DateTime>(nullable: true),
                    OperationArguments = table.Column<string>(nullable: false),
                    OperationResult = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");
        }
    }
}
