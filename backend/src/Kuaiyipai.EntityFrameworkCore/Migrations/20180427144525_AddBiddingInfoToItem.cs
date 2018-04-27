using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Kuaiyipai.Migrations
{
    public partial class AddBiddingInfoToItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BiddingCount",
                table: "AUC_Items_Terminated",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "HighestBiddingPrice",
                table: "AUC_Items_Terminated",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "BiddingCount",
                table: "AUC_Items_Drafting",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "HighestBiddingPrice",
                table: "AUC_Items_Drafting",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "BiddingCount",
                table: "AUC_Items_Completed",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "HighestBiddingPrice",
                table: "AUC_Items_Completed",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "BiddingCount",
                table: "AUC_Items_Auctioning",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "HighestBiddingPrice",
                table: "AUC_Items_Auctioning",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiddingCount",
                table: "AUC_Items_Terminated");

            migrationBuilder.DropColumn(
                name: "HighestBiddingPrice",
                table: "AUC_Items_Terminated");

            migrationBuilder.DropColumn(
                name: "BiddingCount",
                table: "AUC_Items_Drafting");

            migrationBuilder.DropColumn(
                name: "HighestBiddingPrice",
                table: "AUC_Items_Drafting");

            migrationBuilder.DropColumn(
                name: "BiddingCount",
                table: "AUC_Items_Completed");

            migrationBuilder.DropColumn(
                name: "HighestBiddingPrice",
                table: "AUC_Items_Completed");

            migrationBuilder.DropColumn(
                name: "BiddingCount",
                table: "AUC_Items_Auctioning");

            migrationBuilder.DropColumn(
                name: "HighestBiddingPrice",
                table: "AUC_Items_Auctioning");
        }
    }
}
