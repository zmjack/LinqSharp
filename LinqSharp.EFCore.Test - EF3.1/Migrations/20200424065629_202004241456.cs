using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinqSharp.Test.Migrations
{
    public partial class _202004241456 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RandomId",
                table: "SimpleModels",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SimpleModels_RandomId",
                table: "SimpleModels",
                column: "RandomId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SimpleModels_RandomId",
                table: "SimpleModels");

            migrationBuilder.DropColumn(
                name: "RandomId",
                table: "SimpleModels");
        }
    }
}
