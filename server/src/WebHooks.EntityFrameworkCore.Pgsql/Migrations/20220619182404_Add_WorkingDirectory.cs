using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebHooks.EntityFrameworkCore.Pgsql.Migrations
{
    public partial class Add_WorkingDirectory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkingDirectory",
                table: "Works",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkDirectory",
                table: "GiteeConfigs",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GitConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddressType = table.Column<string>(type: "text", nullable: true),
                    RepositoryAddress = table.Column<string>(type: "text", nullable: true),
                    Branch = table.Column<string>(type: "text", nullable: true),
                    Tag = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitConfigs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GitConfigs");

            migrationBuilder.DropColumn(
                name: "WorkingDirectory",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "WorkDirectory",
                table: "GiteeConfigs");
        }
    }
}
