namespace AdsPortal.WebApi.Persistence.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedRunAfter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RunAfterId",
                table: "Jobs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_RunAfterId",
                table: "Jobs",
                column: "RunAfterId",
                unique: true,
                filter: "[RunAfterId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Jobs_RunAfterId",
                table: "Jobs",
                column: "RunAfterId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Jobs_RunAfterId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_RunAfterId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "RunAfterId",
                table: "Jobs");
        }
    }
}
