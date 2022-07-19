using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCreator.Migrations
{
    public partial class _202207131312 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppRegistries_Item_Key",
                table: "AppRegistries");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "AppRegistries",
                maxLength: 127,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(127)",
                oldMaxLength: 127,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Item",
                table: "AppRegistries",
                maxLength: 127,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(127)",
                oldMaxLength: 127,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppRegistries_Item_Key",
                table: "AppRegistries",
                columns: new[] { "Item", "Key" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppRegistries_Item_Key",
                table: "AppRegistries");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "AppRegistries",
                type: "nvarchar(127)",
                maxLength: 127,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 127);

            migrationBuilder.AlterColumn<string>(
                name: "Item",
                table: "AppRegistries",
                type: "nvarchar(127)",
                maxLength: 127,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 127);

            migrationBuilder.CreateIndex(
                name: "IX_AppRegistries_Item_Key",
                table: "AppRegistries",
                columns: new[] { "Item", "Key" },
                unique: true,
                filter: "[Item] IS NOT NULL AND [Key] IS NOT NULL");
        }
    }
}
