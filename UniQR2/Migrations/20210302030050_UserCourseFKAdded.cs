using Microsoft.EntityFrameworkCore.Migrations;

namespace UniQR2.Migrations
{
    public partial class UserCourseFKAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseClassroom_Users_UserID",
                table: "CourseClassroom");

            migrationBuilder.DropIndex(
                name: "IX_CourseClassroom_UserID",
                table: "CourseClassroom");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "CourseClassroom");

            migrationBuilder.AddColumn<int>(
                name: "InstructorID",
                table: "CourseClassroom",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassroom_InstructorID",
                table: "CourseClassroom",
                column: "InstructorID");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseClassroom_Users_InstructorID",
                table: "CourseClassroom",
                column: "InstructorID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseClassroom_Users_InstructorID",
                table: "CourseClassroom");

            migrationBuilder.DropIndex(
                name: "IX_CourseClassroom_InstructorID",
                table: "CourseClassroom");

            migrationBuilder.DropColumn(
                name: "InstructorID",
                table: "CourseClassroom");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "CourseClassroom",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassroom_UserID",
                table: "CourseClassroom",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseClassroom_Users_UserID",
                table: "CourseClassroom",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
