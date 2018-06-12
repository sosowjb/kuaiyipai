using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Kuaiyipai.Migrations
{
    public partial class AddFieldsBalanceRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaidSuccessfully",
                table: "AUC_BalanceRecords",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentCompleteTime",
                table: "AUC_BalanceRecords",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "WechatPayId",
                table: "AUC_BalanceRecords",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaidSuccessfully",
                table: "AUC_BalanceRecords");

            migrationBuilder.DropColumn(
                name: "PaymentCompleteTime",
                table: "AUC_BalanceRecords");

            migrationBuilder.DropColumn(
                name: "WechatPayId",
                table: "AUC_BalanceRecords");
        }
    }
}
