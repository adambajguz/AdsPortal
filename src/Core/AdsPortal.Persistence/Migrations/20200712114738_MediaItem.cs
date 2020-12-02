namespace AdsPortal.Persistence.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class MediaItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Users");

            migrationBuilder.AddColumn<long>(
                name: "Role",
                table: "Users",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    LastSavedOn = table.Column<DateTime>(nullable: false),
                    LastSavedBy = table.Column<Guid>(nullable: true),
                    FileName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Alt = table.Column<string>(nullable: false),
                    VirtualPath = table.Column<string>(nullable: true),
                    Data = table.Column<byte[]>(nullable: true),
                    Hash = table.Column<string>(nullable: true),
                    ByteSize = table.Column<long>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: true),
                    Role = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaItems");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.AddColumn<long>(
                name: "Roles",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
