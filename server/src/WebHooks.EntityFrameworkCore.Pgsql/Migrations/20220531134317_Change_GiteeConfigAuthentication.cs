using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebHooks.EntityFrameworkCore.Pgsql.Migrations
{
    public partial class Change_GiteeConfigAuthentication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Authentications",
                table: "GiteeConfigs",
                newName: "Authentication");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Authentication",
                table: "GiteeConfigs",
                newName: "Authentications");
        }
    }
}
