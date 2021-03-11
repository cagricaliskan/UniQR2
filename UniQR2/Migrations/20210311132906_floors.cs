using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UniQR2.Migrations
{
    public partial class floors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Floors_Classrooms_ClassroomID",
                table: "Floors");

            migrationBuilder.DropIndex(
                name: "IX_Floors_ClassroomID",
                table: "Floors");

            migrationBuilder.DropColumn(
                name: "ClassroomID",
                table: "Floors");

            migrationBuilder.AddColumn<int>(
                name: "FloorID",
                table: "Classrooms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Floors",
                table: "Classrooms",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 3, 11, 17, 29, 6, 330, DateTimeKind.Local).AddTicks(4527));

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_FloorID",
                table: "Classrooms",
                column: "FloorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_Floors_FloorID",
                table: "Classrooms",
                column: "FloorID",
                principalTable: "Floors",
                principalColumn: "FloorID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_Floors_FloorID",
                table: "Classrooms");

            migrationBuilder.DropIndex(
                name: "IX_Classrooms_FloorID",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "FloorID",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "Floors",
                table: "Classrooms");

            migrationBuilder.AddColumn<int>(
                name: "ClassroomID",
                table: "Floors",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 3, 8, 11, 2, 57, 772, DateTimeKind.Local).AddTicks(6967));

            migrationBuilder.CreateIndex(
                name: "IX_Floors_ClassroomID",
                table: "Floors",
                column: "ClassroomID");

            migrationBuilder.AddForeignKey(
                name: "FK_Floors_Classrooms_ClassroomID",
                table: "Floors",
                column: "ClassroomID",
                principalTable: "Classrooms",
                principalColumn: "ClassroomID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
