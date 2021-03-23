using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UniQR2.Migrations
{
    public partial class key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 3, 23, 23, 42, 39, 580, DateTimeKind.Local).AddTicks(2119));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 3, 14, 18, 32, 48, 695, DateTimeKind.Local).AddTicks(1634));
        }
    }
}
