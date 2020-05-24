using Microsoft.EntityFrameworkCore.Migrations;

namespace TelleR.Migrations.Migrations
{
    public partial class AddBlogIntoInvites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BlogId",
                table: "AuthorInvites",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorInvites_BlogId",
                table: "AuthorInvites",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorInvites_Blogs_BlogId",
                table: "AuthorInvites",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorInvites_Blogs_BlogId",
                table: "AuthorInvites");

            migrationBuilder.DropIndex(
                name: "IX_AuthorInvites_BlogId",
                table: "AuthorInvites");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "AuthorInvites");
        }
    }
}
