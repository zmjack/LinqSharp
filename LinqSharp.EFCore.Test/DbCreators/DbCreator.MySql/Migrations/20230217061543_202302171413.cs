using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCreator.Migrations
{
    public partial class _202302171413 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TrackModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "TrackModels",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TrackModels");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TrackModels");
        }
    }
}
