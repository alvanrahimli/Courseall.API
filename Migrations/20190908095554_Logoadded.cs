using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseAll.API.Migrations
{
    public partial class Logoadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Logo",
                table: "Companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Companies");
        }
    }
}
