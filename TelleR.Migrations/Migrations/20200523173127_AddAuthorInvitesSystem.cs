using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TelleR.Migrations.Migrations
{
    public partial class AddAuthorInvitesSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorInvites",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<long>(nullable: false),
                    ReceiverId = table.Column<long>(nullable: false),
                    IsApprove = table.Column<bool>(nullable: true),
                    IsSenderNotified = table.Column<bool>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ReceiverRespondDate = table.Column<DateTime>(nullable: true),
                    SenderViewedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorInvites_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuthorInvites_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorInvites_ReceiverId",
                table: "AuthorInvites",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorInvites_SenderId",
                table: "AuthorInvites",
                column: "SenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorInvites");
        }
    }
}
