using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Kuaiyipai.Migrations
{
    public partial class OrderAddItemId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "AUC_Orders_WaitingForSending",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "AUC_Orders_WaitingForReceiving",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "AUC_Orders_WaitingForPayment",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "AUC_Orders_WaitingForEvaluating",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "AUC_Orders_Completed",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "AUC_Orders_WaitingForSending");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "AUC_Orders_WaitingForReceiving");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "AUC_Orders_WaitingForPayment");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "AUC_Orders_WaitingForEvaluating");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "AUC_Orders_Completed");
        }
    }
}
