using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreRelationshipsTutorial.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RpgClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Userid = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.id);
                    table.ForeignKey(
                        name: "FK_Characters_Users_Userid",
                        column: x => x.Userid,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Damage = table.Column<int>(type: "int", nullable: false),
                    Charactersid = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.id);
                    table.ForeignKey(
                        name: "FK_Skills_Characters_Charactersid",
                        column: x => x.Charactersid,
                        principalTable: "Characters",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Damage = table.Column<int>(type: "int", nullable: false),
                    Characterid = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.id);
                    table.ForeignKey(
                        name: "FK_Weapons_Characters_Characterid",
                        column: x => x.Characterid,
                        principalTable: "Characters",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Elements",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElemType = table.Column<int>(type: "int", nullable: false),
                    Skillid = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elements", x => x.id);
                    table.ForeignKey(
                        name: "FK_Elements_Skills_Skillid",
                        column: x => x.Skillid,
                        principalTable: "Skills",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Userid",
                table: "Characters",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_Skillid",
                table: "Elements",
                column: "Skillid");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Charactersid",
                table: "Skills",
                column: "Charactersid");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_Characterid",
                table: "Weapons",
                column: "Characterid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Elements");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
