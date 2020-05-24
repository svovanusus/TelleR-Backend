using Microsoft.EntityFrameworkCore.Migrations;

namespace TelleR.Migrations.Migrations
{
    public partial class UpdateConfigurationForAuthorInvite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsSenderNotified",
                table: "AuthorInvites",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsSenderNotified",
                table: "AuthorInvites",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
