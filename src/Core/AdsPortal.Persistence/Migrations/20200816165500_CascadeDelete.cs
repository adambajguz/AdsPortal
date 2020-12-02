namespace AdsPortal.Persistence.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class CascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_Journals_JournalId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Degree",
                table: "Authors");

            migrationBuilder.AddColumn<Guid>(
                name: "DegreeId",
                table: "Authors",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Authors",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Degree",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    LastSavedOn = table.Column<DateTime>(nullable: false),
                    LastSavedBy = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degree", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    LastSavedOn = table.Column<DateTime>(nullable: false),
                    LastSavedBy = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_DegreeId",
                table: "Authors",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_DepartmentId",
                table: "Authors",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Degree_DegreeId",
                table: "Authors",
                column: "DegreeId",
                principalTable: "Degree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Department_DepartmentId",
                table: "Authors",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_Journals_JournalId",
                table: "Publications",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Degree_DegreeId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Department_DepartmentId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Publications_Journals_JournalId",
                table: "Publications");

            migrationBuilder.DropTable(
                name: "Degree");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Authors_DegreeId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_DepartmentId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "DegreeId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Authors");

            migrationBuilder.AddColumn<byte>(
                name: "Degree",
                table: "Authors",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_Journals_JournalId",
                table: "Publications",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
