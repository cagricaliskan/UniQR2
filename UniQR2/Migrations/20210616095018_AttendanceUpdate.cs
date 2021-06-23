using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace UniQR2.Migrations
{
    public partial class AttendanceUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseID);
                });

            migrationBuilder.CreateTable(
                name: "Floors",
                columns: table => new
                {
                    FloorID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FloorNum = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floors", x => x.FloorID);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Fullname = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    ResetCode = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    UserRole = table.Column<int>(nullable: false),
                    ResetCodeExpire = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    ClassroomID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    FloorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.ClassroomID);
                    table.ForeignKey(
                        name: "FK_Classrooms_Floors_FloorID",
                        column: x => x.FloorID,
                        principalTable: "Floors",
                        principalColumn: "FloorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseClassrooms",
                columns: table => new
                {
                    CourseClassroomID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ClassroomID = table.Column<int>(nullable: false),
                    CourseID = table.Column<int>(nullable: false),
                    InstructorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseClassrooms", x => x.CourseClassroomID);
                    table.ForeignKey(
                        name: "FK_CourseClassrooms_Classrooms_ClassroomID",
                        column: x => x.ClassroomID,
                        principalTable: "Classrooms",
                        principalColumn: "ClassroomID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseClassrooms_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseClassrooms_Users_InstructorID",
                        column: x => x.InstructorID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    AnnouncementID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Header = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    CourseClassroomID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.AnnouncementID);
                    table.ForeignKey(
                        name: "FK_Announcements_CourseClassrooms_CourseClassroomID",
                        column: x => x.CourseClassroomID,
                        principalTable: "CourseClassrooms",
                        principalColumn: "CourseClassroomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceLists",
                columns: table => new
                {
                    AttendanceListID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    QrString = table.Column<string>(nullable: true),
                    CourseClassroomID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceLists", x => x.AttendanceListID);
                    table.ForeignKey(
                        name: "FK_AttendanceLists_CourseClassrooms_CourseClassroomID",
                        column: x => x.CourseClassroomID,
                        principalTable: "CourseClassrooms",
                        principalColumn: "CourseClassroomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseStudentRels",
                columns: table => new
                {
                    CourseStudentRelID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StudentID = table.Column<int>(nullable: false),
                    ClassroomID = table.Column<int>(nullable: false),
                    CourseClassroomID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStudentRels", x => x.CourseStudentRelID);
                    table.ForeignKey(
                        name: "FK_CourseStudentRels_CourseClassrooms_CourseClassroomID",
                        column: x => x.CourseClassroomID,
                        principalTable: "CourseClassrooms",
                        principalColumn: "CourseClassroomID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseStudentRels_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    FileID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    DataPath = table.Column<string>(nullable: true),
                    CourseClassroomID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.FileID);
                    table.ForeignKey(
                        name: "FK_Files_CourseClassrooms_CourseClassroomID",
                        column: x => x.CourseClassroomID,
                        principalTable: "CourseClassrooms",
                        principalColumn: "CourseClassroomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participations",
                columns: table => new
                {
                    ParticipationID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AttendTime = table.Column<DateTime>(nullable: false),
                    StudentID = table.Column<int>(nullable: false),
                    AttendanceListID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participations", x => x.ParticipationID);
                    table.ForeignKey(
                        name: "FK_Participations_AttendanceLists_AttendanceListID",
                        column: x => x.AttendanceListID,
                        principalTable: "AttendanceLists",
                        principalColumn: "AttendanceListID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participations_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Email", "FullName", "IsActive", "Password", "ResetCode", "ResetCodeExpire", "UserRole" },
                values: new object[] { 1, "kamren2@ethereal.email", "System Admin", true, "123123", null, new DateTime(2021, 6, 16, 13, 50, 18, 453, DateTimeKind.Local).AddTicks(2932), 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CourseClassroomID",
                table: "Announcements",
                column: "CourseClassroomID");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLists_CourseClassroomID",
                table: "AttendanceLists",
                column: "CourseClassroomID");

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_FloorID",
                table: "Classrooms",
                column: "FloorID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassrooms_ClassroomID",
                table: "CourseClassrooms",
                column: "ClassroomID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassrooms_CourseID",
                table: "CourseClassrooms",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassrooms_InstructorID",
                table: "CourseClassrooms",
                column: "InstructorID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudentRels_CourseClassroomID",
                table: "CourseStudentRels",
                column: "CourseClassroomID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudentRels_StudentID",
                table: "CourseStudentRels",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Files_CourseClassroomID",
                table: "Files",
                column: "CourseClassroomID");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_AttendanceListID",
                table: "Participations",
                column: "AttendanceListID");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_StudentID",
                table: "Participations",
                column: "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "CourseStudentRels");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Participations");

            migrationBuilder.DropTable(
                name: "AttendanceLists");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "CourseClassrooms");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Floors");
        }
    }
}
