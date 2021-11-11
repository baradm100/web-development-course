using Microsoft.EntityFrameworkCore.Migrations;

namespace web_development_course.Migrations
{
    public partial class AlignLocalDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningHour_Branch_BranchId",
                table: "OpeningHour");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "OpeningHour",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningHour_Branch_BranchId",
                table: "OpeningHour",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningHour_Branch_BranchId",
                table: "OpeningHour");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "OpeningHour",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningHour_Branch_BranchId",
                table: "OpeningHour",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
