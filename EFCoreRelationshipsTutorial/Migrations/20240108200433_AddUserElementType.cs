using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreRelationshipsTutorial.Migrations
{
    public partial class AddUserElementType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElementType",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElementType",
                table: "Users");
        }
    }
}
