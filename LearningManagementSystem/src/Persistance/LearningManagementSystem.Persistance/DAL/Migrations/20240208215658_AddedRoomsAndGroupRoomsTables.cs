using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Persistance.DAL.Migrations
{
    public partial class AddedRoomsAndGroupRoomsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupRoom_Groups_GroupId",
                table: "GroupRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupRoom_Room_RoomId",
                table: "GroupRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupRoom",
                table: "GroupRoom");

            migrationBuilder.RenameTable(
                name: "GroupRoom",
                newName: "GroupRooms");

            migrationBuilder.RenameIndex(
                name: "IX_GroupRoom_RoomId",
                table: "GroupRooms",
                newName: "IX_GroupRooms_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupRoom_GroupId",
                table: "GroupRooms",
                newName: "IX_GroupRooms_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupRooms",
                table: "GroupRooms",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRooms_Groups_GroupId",
                table: "GroupRooms",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRooms_Room_RoomId",
                table: "GroupRooms",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupRooms_Groups_GroupId",
                table: "GroupRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupRooms_Room_RoomId",
                table: "GroupRooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupRooms",
                table: "GroupRooms");

            migrationBuilder.RenameTable(
                name: "GroupRooms",
                newName: "GroupRoom");

            migrationBuilder.RenameIndex(
                name: "IX_GroupRooms_RoomId",
                table: "GroupRoom",
                newName: "IX_GroupRoom_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupRooms_GroupId",
                table: "GroupRoom",
                newName: "IX_GroupRoom_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupRoom",
                table: "GroupRoom",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRoom_Groups_GroupId",
                table: "GroupRoom",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRoom_Room_RoomId",
                table: "GroupRoom",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
