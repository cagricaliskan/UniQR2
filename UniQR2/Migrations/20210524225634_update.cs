using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UniQR2.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndHour",
                table: "AttendanceLists");

            migrationBuilder.DropColumn(
                name: "StartHour",
                table: "AttendanceLists");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 5, 25, 2, 56, 34, 391, DateTimeKind.Local).AddTicks(7600));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndHour",
                table: "AttendanceLists",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartHour",
                table: "AttendanceLists",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 5, 25, 2, 55, 15, 344, DateTimeKind.Local).AddTicks(5586));
        }
    }
}
