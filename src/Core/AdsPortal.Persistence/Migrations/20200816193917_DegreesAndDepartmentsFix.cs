using Microsoft.EntityFrameworkCore.Migrations;

namespace AdsPortal.Persistence.Migrations
{
    public partial class DegreesAndDepartmentsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Degree_DegreeId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Department_DepartmentId",
                table: "Authors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Department",
                table: "Department");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Degree",
                table: "Degree");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "Departments");

            migrationBuilder.RenameTable(
                name: "Degree",
                newName: "Degrees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Degrees",
                table: "Degrees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Degrees_DegreeId",
                table: "Authors",
                column: "DegreeId",
                principalTable: "Degrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Departments_DepartmentId",
                table: "Authors",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Degrees_DegreeId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Departments_DepartmentId",
                table: "Authors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Degrees",
                table: "Degrees");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Department");

            migrationBuilder.RenameTable(
                name: "Degrees",
                newName: "Degree");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Department",
                table: "Department",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Degree",
                table: "Degree",
                column: "Id");

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
        }
    }
}
