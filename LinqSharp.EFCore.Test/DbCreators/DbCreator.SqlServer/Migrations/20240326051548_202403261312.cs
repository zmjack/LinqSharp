using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbCreator.Migrations
{
    /// <inheritdoc />
    public partial class _202403261312 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackModels");

            migrationBuilder.CreateTable(
                name: "AutoModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastWriteTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Month_DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Month_DateTimeOffset = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Month_NullableDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Month_NullableDateTimeOffset = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Trim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Upper = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lower = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Condensed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Even = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoModels", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoModels");

            migrationBuilder.CreateTable(
                name: "TrackModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ForCondensed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForEven = table.Column<int>(type: "int", nullable: false),
                    ForLower = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForTrim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForUpper = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastWriteTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackModels", x => x.Id);
                });
        }
    }
}
