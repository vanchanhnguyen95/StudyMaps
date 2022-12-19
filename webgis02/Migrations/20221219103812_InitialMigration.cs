using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace webgis02.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HaLongRoads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ClassFunc = table.Column<short>(type: "smallint", nullable: true),
                    Level = table.Column<short>(type: "smallint", nullable: true),
                    Kind = table.Column<short>(type: "smallint", nullable: true),
                    Minspeed = table.Column<short>(type: "smallint", nullable: true),
                    MaxSpeed = table.Column<short>(type: "smallint", nullable: true),
                    ProvinceID = table.Column<short>(type: "smallint", nullable: true),
                    SegmentID = table.Column<long>(type: "bigint", nullable: true),
                    Coordinates = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HaLongRoads", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HaLongRoads");
        }
    }
}
