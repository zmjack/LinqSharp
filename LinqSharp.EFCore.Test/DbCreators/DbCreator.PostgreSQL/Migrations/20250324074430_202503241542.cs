using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbCreator.Migrations
{
    /// <inheritdoc />
    public partial class _202503241542 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZipperModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KeyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    ZipperStart = table.Column<DateOnly>(type: "date", nullable: false),
                    ZipperEnd = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZipperModels", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZipperModels");
        }
    }
}
