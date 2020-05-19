using Microsoft.EntityFrameworkCore.Migrations;

namespace TelleR.Migrations.Migrations
{
    public partial class AddBlogNameField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Blogs",
                maxLength: 20,
                nullable: false,
                defaultValue: "blogname");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Blogs");
        }
    }
}
