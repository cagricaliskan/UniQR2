﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UniQR2.Models;

namespace UniQR2.Migrations
{
    [DbContext(typeof(ModelContext))]
    [Migration("20210301203348_User2")]
    partial class User2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("UniQR2.Models.AttendanceList", b =>
                {
                    b.Property<int>("AttendanceListID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CourseClassroomID")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("AttendanceListID");

                    b.HasIndex("CourseClassroomID");

                    b.ToTable("AttendanceList");
                });

            modelBuilder.Entity("UniQR2.Models.Classroom", b =>
                {
                    b.Property<int>("ClassroomID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("ClassroomID");

                    b.ToTable("Classroom");
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

                    b.ToTable("Course");
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

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("CourseClassroomID");

                    b.HasIndex("ClassroomID");

                    b.HasIndex("CourseID");

                    b.HasIndex("UserID");

                    b.ToTable("CourseClassroom");
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

                    b.ToTable("CourseStudentRel");
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

                    b.ToTable("Participation");
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

                    b.ToTable("Student");
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

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("ResetCode")
                        .HasColumnType("text");

                    b.Property<DateTime>("ResetCodeExpire")
                        .HasColumnType("datetime");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.Property<string>("activationCode")
                        .HasColumnType("text");

                    b.Property<bool>("isActive")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UniQR2.Models.AttendanceList", b =>
                {
                    b.HasOne("UniQR2.Models.CourseClassroom", "CourseClassroom")
                        .WithMany("AttendanceLists")
                        .HasForeignKey("CourseClassroomID")
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

                    b.HasOne("UniQR2.Models.User", null)
                        .WithMany("CourseClassrooms")
                        .HasForeignKey("UserID");
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
