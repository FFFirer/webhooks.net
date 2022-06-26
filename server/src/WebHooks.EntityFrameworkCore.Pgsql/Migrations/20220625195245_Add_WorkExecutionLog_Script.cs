using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebHooks.EntityFrameworkCore.Pgsql.Migrations
{
    public partial class Add_WorkExecutionLog_Script : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Scripts",
                table: "BuildScripts",
                newName: "Script");

            migrationBuilder.AddColumn<List<string>>(
                name: "Script",
                table: "WorkExecutionLogs",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Script",
                table: "WorkExecutionLogs");

            migrationBuilder.RenameColumn(
                name: "Script",
                table: "BuildScripts",
                newName: "Scripts");
        }
    }
}
