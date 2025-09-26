using Microsoft.EntityFrameworkCore.Migrations;

namespace Finsight.Core.Migrations
{
    public partial class setnull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_ExecutorId",
                table: "Order");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_ExecutorId",
                table: "Order",
                column: "ExecutorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_ExecutorId",
                table: "Order");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_ExecutorId",
                table: "Order",
                column: "ExecutorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
