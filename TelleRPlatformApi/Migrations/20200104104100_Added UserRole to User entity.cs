using Microsoft.EntityFrameworkCore.Migrations;
using TelleRPlatformApi.Models;

namespace TelleRPlatformApi.Migrations
{
    public partial class AddedUserRoletoUserentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                nullable: false,
                defaultValue: UserRole.User);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");
        }
    }
}
