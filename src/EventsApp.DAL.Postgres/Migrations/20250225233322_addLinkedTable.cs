using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventsApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addLinkedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventEntityUserEntity_Users_ParticipantsId",
                table: "EventEntityUserEntity");

            migrationBuilder.RenameColumn(
                name: "ParticipantsId",
                table: "EventEntityUserEntity",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_EventEntityUserEntity_ParticipantsId",
                table: "EventEntityUserEntity",
                newName: "IX_EventEntityUserEntity_UsersId");

            migrationBuilder.CreateTable(
                name: "EventUsers",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventUsers", x => new { x.EventId, x.UserId });
                    table.ForeignKey(
                        name: "FK_EventUsers_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventUsers_UserId",
                table: "EventUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventEntityUserEntity_Users_UsersId",
                table: "EventEntityUserEntity",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventEntityUserEntity_Users_UsersId",
                table: "EventEntityUserEntity");

            migrationBuilder.DropTable(
                name: "EventUsers");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "EventEntityUserEntity",
                newName: "ParticipantsId");

            migrationBuilder.RenameIndex(
                name: "IX_EventEntityUserEntity_UsersId",
                table: "EventEntityUserEntity",
                newName: "IX_EventEntityUserEntity_ParticipantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventEntityUserEntity_Users_ParticipantsId",
                table: "EventEntityUserEntity",
                column: "ParticipantsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
