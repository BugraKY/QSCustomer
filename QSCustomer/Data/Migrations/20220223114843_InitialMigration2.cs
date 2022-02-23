using Microsoft.EntityFrameworkCore.Migrations;

namespace QSCustomer.Data.Migrations
{
    public partial class InitialMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    TypeNumber = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTypes");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AspNetUsers");
        }
    }
}
