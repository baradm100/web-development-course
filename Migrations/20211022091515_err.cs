using Microsoft.EntityFrameworkCore.Migrations;

namespace web_development_course.Migrations
{
    public partial class err : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "delivery",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "delivery",
                table: "Order");
        }
    }
}
