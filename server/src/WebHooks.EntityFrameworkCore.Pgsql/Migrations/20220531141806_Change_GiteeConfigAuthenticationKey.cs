using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebHooks.EntityFrameworkCore.Pgsql.Migrations
{
    public partial class Change_GiteeConfigAuthenticationKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Secret",
                table: "GiteeConfigs");

            migrationBuilder.RenameColumn(
                name: "SignatureKey",
                table: "GiteeConfigs",
                newName: "AuthenticationKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthenticationKey",
                table: "GiteeConfigs",
                newName: "SignatureKey");

            migrationBuilder.AddColumn<string>(
                name: "Secret",
                table: "GiteeConfigs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
