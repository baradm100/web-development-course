using Microsoft.EntityFrameworkCore.Migrations;

namespace web_development_course.Migrations
{
    public partial class address_users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "delivery",
                table: "Order",
                newName: "Delivery");

            migrationBuilder.CreateTable(
                name: "AddressUser",
                columns: table => new
                {
                    AddressesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressUser", x => new { x.AddressesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AddressUser_Address_AddressesId",
                        column: x => x.AddressesId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddressUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressUser_UsersId",
                table: "AddressUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressUser");

            migrationBuilder.RenameColumn(
                name: "Delivery",
                table: "Order",
                newName: "delivery");
        }
    }
}
