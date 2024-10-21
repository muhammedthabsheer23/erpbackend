using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loginproject.Migrations
{
    /// <inheritdoc />
    public partial class createpaymod4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "paymods");

            migrationBuilder.CreateTable(
                name: "PAYMODT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Post_ledger = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Post_Ledger_id = table.Column<int>(type: "int", nullable: false),
                    Company_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYMODT", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PAYMODT");

            migrationBuilder.CreateTable(
                name: "paymods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Post_Ledger_id = table.Column<int>(type: "int", nullable: false),
                    Post_ledger = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymods", x => x.Id);
                });
        }
    }
}
