using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.CodeFirst.Migrations
{
    public partial class StudentTeacherFluentAPI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentTeacher_Students_StudentsId",
                table: "StudentTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentTeacher_Teachers_TeachersId",
                table: "StudentTeacher");

            migrationBuilder.RenameColumn(
                name: "TeachersId",
                table: "StudentTeacher",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "StudentsId",
                table: "StudentTeacher",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentTeacher_TeachersId",
                table: "StudentTeacher",
                newName: "IX_StudentTeacher_TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentId",
                table: "StudentTeacher",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherId",
                table: "StudentTeacher",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentId",
                table: "StudentTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherId",
                table: "StudentTeacher");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "StudentTeacher",
                newName: "TeachersId");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "StudentTeacher",
                newName: "StudentsId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentTeacher_TeacherId",
                table: "StudentTeacher",
                newName: "IX_StudentTeacher_TeachersId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTeacher_Students_StudentsId",
                table: "StudentTeacher",
                column: "StudentsId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTeacher_Teachers_TeachersId",
                table: "StudentTeacher",
                column: "TeachersId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
