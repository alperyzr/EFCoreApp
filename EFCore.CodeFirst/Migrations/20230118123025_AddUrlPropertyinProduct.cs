using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.CodeFirst.Migrations
{
    /// <inheritdoc />
    public partial class AddUrlPropertyinProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "char(100)",
                unicode: false,
                fixedLength: true,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(100)",
                oldFixedLength: true,
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Products",
                type: "varchar(200)",
                unicode: false,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductFulls",
                columns: table => new
                {
                    CategoryId = table.Column<int>(name: "Category_Id", type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(name: "Product_Id", type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ProductFeatureId = table.Column<int>(name: "ProductFeature_Id", type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductFulls");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nchar(100)",
                fixedLength: true,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(100)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "Barcode",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
