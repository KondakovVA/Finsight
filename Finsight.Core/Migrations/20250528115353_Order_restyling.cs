using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Finsight.Core.Migrations
{
    public partial class Order_restyling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_AddedById",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_AddedById",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "AddedById",
                table: "Order");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentsPath",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Order",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DocumentsPath",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Order");

            migrationBuilder.AddColumn<Guid>(
                name: "AddedById",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Order_AddedById",
                table: "Order",
                column: "AddedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_AddedById",
                table: "Order",
                column: "AddedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
