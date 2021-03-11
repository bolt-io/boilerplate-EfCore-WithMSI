using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.EFCore.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sample");

            migrationBuilder.CreateTable(
                name: "Entities",
                schema: "sample",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExampleProperty = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true, comment: "This is an example of a column comment.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entities",
                schema: "sample");
        }
    }
}
