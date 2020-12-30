using Microsoft.EntityFrameworkCore.Migrations;

namespace AdsPortal.Persistence.Migrations
{
    public partial class JobNoUlong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "JobNo",
                table: "Jobs",
                type: "decimal(20,0)",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR Job_JobNo_Sequence",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValueSql: "NEXT VALUE FOR Job_JobNo_Sequence");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "JobNo",
                table: "Jobs",
                type: "bigint",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR Job_JobNo_Sequence",
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)",
                oldDefaultValueSql: "NEXT VALUE FOR Job_JobNo_Sequence");
        }
    }
}
