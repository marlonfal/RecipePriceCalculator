using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class Addrecipetables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    ProductTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.ProductTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    IsOrganic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "ProductTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeProducts",
                columns: table => new
                {
                    RecipeProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    RecipeId = table.Column<int>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeProducts", x => x.RecipeProductId);
                    table.ForeignKey(
                        name: "FK_RecipeProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeProducts_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "ProductTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Produce" },
                    { 2, "Meat/Poultry" },
                    { 3, "Pantry" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "Name" },
                values: new object[,]
                {
                    { 1, "Recipe 1" },
                    { 2, "Recipe 2" },
                    { 3, "Recipe 3" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "IsOrganic", "Name", "Price", "ProductTypeId" },
                values: new object[,]
                {
                    { 1, true, "Clove of organic garlic", 0.67m, 1 },
                    { 2, false, "Lemon", 2.03m, 1 },
                    { 3, false, "Cup of corn", 0.87m, 1 },
                    { 4, false, "Chicken breast", 2.19m, 2 },
                    { 5, false, "Slice of bacon", 0.24m, 2 },
                    { 6, false, "Ounce of pasta", 0.31m, 3 },
                    { 7, true, "Cup of organic olive oil", 1.92m, 3 },
                    { 8, false, "Cup of vinegar", 1.26m, 3 },
                    { 9, false, "Teaspoon of salt", 0.16m, 3 },
                    { 10, false, "Teaspoon of pepper", 0.17m, 3 }
                });

            migrationBuilder.InsertData(
                table: "RecipeProducts",
                columns: new[] { "RecipeProductId", "ProductId", "Quantity", "RecipeId" },
                values: new object[,]
                {
                    { 1, 1, 1m, 1 },
                    { 6, 1, 1m, 2 },
                    { 11, 1, 1m, 3 },
                    { 2, 2, 1m, 1 },
                    { 12, 3, 4m, 3 },
                    { 7, 4, 4m, 2 },
                    { 13, 5, 4m, 3 },
                    { 14, 6, 8m, 3 },
                    { 3, 7, 0.75m, 1 },
                    { 8, 7, 0.5m, 2 },
                    { 15, 7, 0.33m, 3 },
                    { 9, 8, 0.5m, 2 },
                    { 4, 9, 0.75m, 1 },
                    { 16, 9, 1m, 3 },
                    { 5, 10, 0.5m, 1 },
                    { 17, 10, 0.75m, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeId",
                table: "Products",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeProducts_ProductId",
                table: "RecipeProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeProducts_RecipeId",
                table: "RecipeProducts",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeProducts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "ProductTypes");
        }
    }
}
