using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreRelationshipsTutorial.Migrations
{
    public partial class AddListCharacter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Characters_Charactersid",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_Charactersid",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Charactersid",
                table: "Skills");

            migrationBuilder.AlterColumn<bool>(
                name: "isMale",
                table: "Users",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "CharacterSkill",
                columns: table => new
                {
                    Charactersid = table.Column<int>(type: "int", nullable: false),
                    Skillsid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSkill", x => new { x.Charactersid, x.Skillsid });
                    table.ForeignKey(
                        name: "FK_CharacterSkill_Characters_Charactersid",
                        column: x => x.Charactersid,
                        principalTable: "Characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterSkill_Skills_Skillsid",
                        column: x => x.Skillsid,
                        principalTable: "Skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSkill_Skillsid",
                table: "CharacterSkill",
                column: "Skillsid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterSkill");

            migrationBuilder.AlterColumn<bool>(
                name: "isMale",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Charactersid",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Charactersid",
                table: "Skills",
                column: "Charactersid");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Characters_Charactersid",
                table: "Skills",
                column: "Charactersid",
                principalTable: "Characters",
                principalColumn: "id");
        }
    }
}
