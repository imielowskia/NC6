using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NC6.Migrations
{
    public partial class AddFaculty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacultyId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_FacultyId",
                table: "Groups",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Faculties_FacultyId",
                table: "Groups",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Faculties_FacultyId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropIndex(
                name: "IX_Groups_FacultyId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "Groups");
        }
    }
}
