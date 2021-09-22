using Microsoft.EntityFrameworkCore.Migrations;

namespace web_development_course.Migrations
{
    public partial class Color : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "ProductType");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "ProductType",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductColor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductColor", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ProductColor",
                columns: new[] { "Id", "Color", "Name" },
                values: new object[,]
                {
                    { 1, "EE2A00", "Red" },
                    { 2, "000000", "Black" },
                    { 3, "FFFFFF", "White" },
                    { 4, "B9B9B9", "Grey" },
                    { 5, "FFF704", "Yellow" },
                    { 6, "BF08E3", "Purple" },
                    { 7, "0851E3", "Blue" },
                    { 8, "26E308", "Green" },
                    { 9, "E308CF", "Pink" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductType_ColorId",
                table: "ProductType",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductType_ProductColor_ColorId",
                table: "ProductType",
                column: "ColorId",
                principalTable: "ProductColor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductType_ProductColor_ColorId",
                table: "ProductType");

            migrationBuilder.DropTable(
                name: "ProductColor");

            migrationBuilder.DropIndex(
                name: "IX_ProductType_ColorId",
                table: "ProductType");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "ProductType");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ProductType",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
