using Microsoft.EntityFrameworkCore.Migrations;

namespace LinqSharp.Test.Migrations
{
    public partial class _202004271325 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CheckThrow",
                table: "SimpleModels",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "CheckStoreWin",
                table: "SimpleModels",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "CheckDefault",
                table: "SimpleModels",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "CheckCombine",
                table: "SimpleModels",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "CheckClientWin",
                table: "SimpleModels",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "CheckThrow",
                table: "SimpleModels",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "CheckStoreWin",
                table: "SimpleModels",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "CheckDefault",
                table: "SimpleModels",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "CheckCombine",
                table: "SimpleModels",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "CheckClientWin",
                table: "SimpleModels",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
