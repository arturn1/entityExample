using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreRelationshipsTutorial.Migrations
{
    public partial class AddIsMale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isMale",
                table: "Users",
                type: "bit",
                nullable: true,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isMale",
                table: "Users");
        }
    }
}
