using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebHooks.EntityFrameworkCore.Pgsql.Migrations
{
    public partial class Add_WorkExecutionLog_ElapsedTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "ElapsedTime",
                table: "WorkExecutionLogs",
                type: "interval",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElapsedTime",
                table: "WorkExecutionLogs");
        }
    }
}
