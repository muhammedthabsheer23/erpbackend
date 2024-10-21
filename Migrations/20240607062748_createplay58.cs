using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loginproject.Migrations
{
    /// <inheritdoc />
    public partial class createplay58 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblSalesdetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    invdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    netamount = table.Column<int>(type: "int", nullable: false),
                    cust_id = table.Column<long>(type: "bigint", nullable: false),
                    Salesman = table.Column<int>(type: "int", nullable: false),
                    paymode = table.Column<int>(type: "int", nullable: false),
                    custname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSalesdetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblSalesdetails");
        }
    }
}
