﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace UniQR2.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classroom",
                columns: table => new
                {
                    ClassroomID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Floor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classroom", x => x.ClassroomID);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseID);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Fullname = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "CourseClassroom",
                columns: table => new
                {
                    CourseClassroomID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ClassroomID = table.Column<int>(nullable: false),
                    CourseID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseClassroom", x => x.CourseClassroomID);
                    table.ForeignKey(
                        name: "FK_CourseClassroom_Classroom_ClassroomID",
                        column: x => x.ClassroomID,
                        principalTable: "Classroom",
                        principalColumn: "ClassroomID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseClassroom_Course_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Course",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseClassroom_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceList",
                columns: table => new
                {
                    AttendanceListID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CourseClassroomID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceList", x => x.AttendanceListID);
                    table.ForeignKey(
                        name: "FK_AttendanceList_CourseClassroom_CourseClassroomID",
                        column: x => x.CourseClassroomID,
                        principalTable: "CourseClassroom",
                        principalColumn: "CourseClassroomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseStudentRel",
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
                    table.PrimaryKey("PK_CourseStudentRel", x => x.CourseStudentRelID);
                    table.ForeignKey(
                        name: "FK_CourseStudentRel_CourseClassroom_CourseClassroomID",
                        column: x => x.CourseClassroomID,
                        principalTable: "CourseClassroom",
                        principalColumn: "CourseClassroomID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseStudentRel_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participation",
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
                    table.PrimaryKey("PK_Participation", x => x.ParticipationID);
                    table.ForeignKey(
                        name: "FK_Participation_AttendanceList_AttendanceListID",
                        column: x => x.AttendanceListID,
                        principalTable: "AttendanceList",
                        principalColumn: "AttendanceListID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participation_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceList_CourseClassroomID",
                table: "AttendanceList",
                column: "CourseClassroomID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassroom_ClassroomID",
                table: "CourseClassroom",
                column: "ClassroomID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassroom_CourseID",
                table: "CourseClassroom",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseClassroom_UserID",
                table: "CourseClassroom",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudentRel_CourseClassroomID",
                table: "CourseStudentRel",
                column: "CourseClassroomID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudentRel_StudentID",
                table: "CourseStudentRel",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Participation_AttendanceListID",
                table: "Participation",
                column: "AttendanceListID");

            migrationBuilder.CreateIndex(
                name: "IX_Participation_StudentID",
                table: "Participation",
                column: "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseStudentRel");

            migrationBuilder.DropTable(
                name: "Participation");

            migrationBuilder.DropTable(
                name: "AttendanceList");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "CourseClassroom");

            migrationBuilder.DropTable(
                name: "Classroom");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}