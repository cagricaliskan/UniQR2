using Microsoft.EntityFrameworkCore.Migrations;

namespace UniQR2.Migrations
{
    public partial class UserActivationCodeRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activationCode",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "activationCode",
                table: "Users",
                type: "text",
                nullable: true);
        }
    }
}
