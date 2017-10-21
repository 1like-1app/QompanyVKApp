using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace QompanyVKApp.Migrations
{
    public partial class Cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_MeetingRooms_MeetingRoomId",
                table: "Meetings");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_MeetingRooms_MeetingRoomId",
                table: "Meetings",
                column: "MeetingRoomId",
                principalTable: "MeetingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_MeetingRooms_MeetingRoomId",
                table: "Meetings");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_MeetingRooms_MeetingRoomId",
                table: "Meetings",
                column: "MeetingRoomId",
                principalTable: "MeetingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
