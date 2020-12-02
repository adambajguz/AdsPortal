namespace AdsPortal.Persistence.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class JobNoModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "Job_JobNo_Sequence");

            migrationBuilder.AlterColumn<long>(
                name: "JobNo",
                table: "Jobs",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR Job_JobNo_Sequence",
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "Job_JobNo_Sequence");

            migrationBuilder.AlterColumn<long>(
                name: "JobNo",
                table: "Jobs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValueSql: "NEXT VALUE FOR Job_JobNo_Sequence");
        }
    }
}
