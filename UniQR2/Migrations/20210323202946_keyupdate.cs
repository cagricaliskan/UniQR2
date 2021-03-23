using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UniQR2.Migrations
{
    public partial class keyupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 3, 24, 0, 29, 46, 343, DateTimeKind.Local).AddTicks(2077));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 3, 23, 23, 42, 39, 580, DateTimeKind.Local).AddTicks(2119));
        }
    }
}
