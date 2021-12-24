using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class Addparametertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    ParameterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.ParameterId);
                });

            migrationBuilder.InsertData(
                table: "Parameters",
                columns: new[] { "ParameterId", "Key", "Value" },
                values: new object[] { 1, "SaleTax", "8.6" });

            migrationBuilder.InsertData(
                table: "Parameters",
                columns: new[] { "ParameterId", "Key", "Value" },
                values: new object[] { 2, "WellnessDiscount", "5" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parameters");
        }
    }
}
