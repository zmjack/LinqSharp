using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCreator.Migrations
{
    public partial class _202207131312 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "AppRegistries",
                maxLength: 127,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(127) CHARACTER SET utf8mb4",
                oldMaxLength: 127,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Item",
                table: "AppRegistries",
                maxLength: 127,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(127) CHARACTER SET utf8mb4",
                oldMaxLength: 127,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "AppRegistries",
                type: "varchar(127) CHARACTER SET utf8mb4",
                maxLength: 127,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 127);

            migrationBuilder.AlterColumn<string>(
                name: "Item",
                table: "AppRegistries",
                type: "varchar(127) CHARACTER SET utf8mb4",
                maxLength: 127,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 127);
        }
    }
}
