using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UniQR2.Migrations
{
    public partial class fileupd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 4, 21, 16, 30, 7, 156, DateTimeKind.Local).AddTicks(610));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 4, 21, 14, 57, 9, 369, DateTimeKind.Local).AddTicks(2584));
        }
    }
}
