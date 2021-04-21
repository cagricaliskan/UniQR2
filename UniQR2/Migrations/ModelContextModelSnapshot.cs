﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UniQR2.Models;

namespace UniQR2.Migrations
{
    [DbContext(typeof(ModelContext))]
    partial class ModelContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("UniQR2.Models.Announcement", b =>
                {
                    b.Property<int>("AnnouncementID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CourseClassroomID")
                        .HasColumnType("int");

                    b.Property<string>("Header")
                        .HasColumnType("text");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.HasKey("AnnouncementID");

                    b.HasIndex("CourseClassroomID");

                    b.ToTable("Announcement");
                });

            modelBuilder.Entity("UniQR2.Models.AttendanceList", b =>
                {
                    b.Property<int>("AttendanceListID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CourseClassroomID")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<bool>("Repeat")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("AttendanceListID");

                    b.HasIndex("CourseClassroomID");

                    b.ToTable("AttendanceLists");
                });

            modelBuilder.Entity("UniQR2.Models.Classroom", b =>
                {
                    b.Property<int>("ClassroomID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("FloorID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("ClassroomID");

                    b.HasIndex("FloorID");

                    b.ToTable("Classrooms");
                });

            modelBuilder.Entity("UniQR2.Models.Course", b =>
                {
                    b.Property<int>("CourseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CourseID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("UniQR2.Models.CourseClassroom", b =>
                {
                    b.Property<int>("CourseClassroomID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClassroomID")
                        .HasColumnType("int");

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<int>("InstructorID")
                        .HasColumnType("int");

                    b.HasKey("CourseClassroomID");

                    b.HasIndex("ClassroomID");

                    b.HasIndex("CourseID");

                    b.HasIndex("InstructorID");

                    b.ToTable("CourseClassrooms");
                });

            modelBuilder.Entity("UniQR2.Models.CourseStudentRel", b =>
                {
                    b.Property<int>("CourseStudentRelID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClassroomID")
                        .HasColumnType("int");

                    b.Property<int?>("CourseClassroomID")
                        .HasColumnType("int");

                    b.Property<int>("StudentID")
                        .HasColumnType("int");

                    b.HasKey("CourseStudentRelID");

                    b.HasIndex("CourseClassroomID");

                    b.HasIndex("StudentID");

                    b.ToTable("CourseStudentRels");
                });

            modelBuilder.Entity("UniQR2.Models.File", b =>
                {
                    b.Property<int>("FileID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CourseClassroomID")
                        .HasColumnType("int");

                    b.Property<string>("DataPath")
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<string>("FileType")
                        .HasColumnType("text");

                    b.HasKey("FileID");

                    b.HasIndex("CourseClassroomID");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("UniQR2.Models.Floor", b =>
                {
                    b.Property<int>("FloorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FloorNum")
                        .HasColumnType("text");

                    b.HasKey("FloorID");

                    b.ToTable("Floors");
                });

            modelBuilder.Entity("UniQR2.Models.Participation", b =>
                {
                    b.Property<int>("ParticipationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("AttendTime")
                        .HasColumnType("datetime");

                    b.Property<int>("AttendanceListID")
                        .HasColumnType("int");

                    b.Property<int>("StudentID")
                        .HasColumnType("int");

                    b.HasKey("ParticipationID");

                    b.HasIndex("AttendanceListID");

                    b.HasIndex("StudentID");

                    b.ToTable("Participations");
                });

            modelBuilder.Entity("UniQR2.Models.Student", b =>
                {
                    b.Property<int>("StudentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Fullname")
                        .HasColumnType("text");

                    b.Property<string>("Number")
                        .HasColumnType("text");

                    b.HasKey("StudentID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("UniQR2.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("ResetCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ResetCodeExpire")
                        .HasColumnType("datetime");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.HasKey("UserID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserID = 1,
                            Email = "kamren2@ethereal.email",
                            FullName = "System Admin",
                            IsActive = true,
                            Password = "123123",
                            ResetCodeExpire = new DateTime(2021, 4, 21, 17, 6, 33, 946, DateTimeKind.Local).AddTicks(5499),
                            UserRole = 0
                        });
                });

            modelBuilder.Entity("UniQR2.Models.Announcement", b =>
                {
                    b.HasOne("UniQR2.Models.CourseClassroom", "CourseClassroom")
                        .WithMany("Announcements")
                        .HasForeignKey("CourseClassroomID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UniQR2.Models.AttendanceList", b =>
                {
                    b.HasOne("UniQR2.Models.CourseClassroom", "CourseClassroom")
                        .WithMany("AttendanceLists")
                        .HasForeignKey("CourseClassroomID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UniQR2.Models.Classroom", b =>
                {
                    b.HasOne("UniQR2.Models.Floor", "Floor")
                        .WithMany("Classrooms")
                        .HasForeignKey("FloorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UniQR2.Models.CourseClassroom", b =>
                {
                    b.HasOne("UniQR2.Models.Classroom", "Classroom")
                        .WithMany("CourseClassrooms")
                        .HasForeignKey("ClassroomID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniQR2.Models.Course", "Course")
                        .WithMany("CourseClassrooms")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniQR2.Models.User", "Instructor")
                        .WithMany("CourseClassrooms")
                        .HasForeignKey("InstructorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UniQR2.Models.CourseStudentRel", b =>
                {
                    b.HasOne("UniQR2.Models.CourseClassroom", "CourseClassroom")
                        .WithMany("CourseStudentRels")
                        .HasForeignKey("CourseClassroomID");

                    b.HasOne("UniQR2.Models.Student", "Student")
                        .WithMany("CourseStudentRels")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UniQR2.Models.File", b =>
                {
                    b.HasOne("UniQR2.Models.CourseClassroom", "CourseClassroom")
                        .WithMany("Files")
                        .HasForeignKey("CourseClassroomID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UniQR2.Models.Participation", b =>
                {
                    b.HasOne("UniQR2.Models.AttendanceList", "AttendanceList")
                        .WithMany("Participations")
                        .HasForeignKey("AttendanceListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniQR2.Models.Student", "Student")
                        .WithMany("Participations")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
