using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UniQR2.Migrations
{
    public partial class filebdupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataFile",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "DataPath",
                table: "Files",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 4, 9, 17, 47, 5, 489, DateTimeKind.Local).AddTicks(6646));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataPath",
                table: "Files");

            migrationBuilder.AddColumn<byte[]>(
                name: "DataFile",
                table: "Files",
                type: "varbinary(4000)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "ResetCodeExpire",
                value: new DateTime(2021, 4, 8, 16, 4, 28, 479, DateTimeKind.Local).AddTicks(2824));
        }
    }
}
