using Microsoft.EntityFrameworkCore.Migrations;

namespace LinqSharp.Test.Migrations
{
    public partial class _202004171957 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Automatic",
                table: "TrackModels");

            migrationBuilder.AddColumn<string>(
                name: "FreeModel",
                table: "ProviderTestModels",
                maxLength: 127,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "AppRegistries",
                maxLength: 127,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Item",
                table: "AppRegistries",
                maxLength: 127,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreeModel",
                table: "ProviderTestModels");

            migrationBuilder.AddColumn<string>(
                name: "Automatic",
                table: "TrackModels",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "AppRegistries",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 127,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Item",
                table: "AppRegistries",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 127,
                oldNullable: true);
        }
    }
}
