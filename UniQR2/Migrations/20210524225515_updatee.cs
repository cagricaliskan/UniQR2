using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UniQR2.Migrations
{
    public partial class updatee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Repeat",
                table: "AttendanceLists");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 5, 25, 2, 55, 15, 344, DateTimeKind.Local).AddTicks(5586));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Repeat",
                table: "AttendanceLists",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 5, 15, 0, 47, 7, 25, DateTimeKind.Local).AddTicks(4463));
        }
    }
}
