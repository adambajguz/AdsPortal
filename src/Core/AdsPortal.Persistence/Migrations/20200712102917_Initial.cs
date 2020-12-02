namespace AdsPortal.Persistence.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    LastSavedOn = table.Column<DateTime>(nullable: false),
                    LastSavedBy = table.Column<Guid>(nullable: true),
                    Degree = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: false),
                    ORCID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    TableName = table.Column<string>(nullable: false),
                    Key = table.Column<Guid>(nullable: false),
                    Action = table.Column<int>(nullable: false),
                    Values = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Journals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    LastSavedOn = table.Column<DateTime>(nullable: false),
                    LastSavedBy = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Points = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    LastSavedOn = table.Column<DateTime>(nullable: false),
                    LastSavedBy = table.Column<Guid>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Roles = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    LastSavedOn = table.Column<DateTime>(nullable: false),
                    LastSavedBy = table.Column<Guid>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    JournalId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publications_Journals_JournalId",
                        column: x => x.JournalId,
                        principalTable: "Journals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublicationAuthors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    LastSavedOn = table.Column<DateTime>(nullable: false),
                    LastSavedBy = table.Column<Guid>(nullable: true),
                    AuthorId = table.Column<Guid>(nullable: false),
                    PublicationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationAuthors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicationAuthors_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicationAuthors_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityAuditLogs_Key",
                table: "EntityAuditLogs",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_PublicationAuthors_AuthorId",
                table: "PublicationAuthors",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicationAuthors_PublicationId",
                table: "PublicationAuthors",
                column: "PublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_JournalId",
                table: "Publications",
                column: "JournalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityAuditLogs");

            migrationBuilder.DropTable(
                name: "PublicationAuthors");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Publications");

            migrationBuilder.DropTable(
                name: "Journals");
        }
    }
}
