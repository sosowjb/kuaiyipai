using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Kuaiyipai.Migrations
{
    public partial class OrderChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedTime",
                table: "AUC_Orders_WaitingForSending",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EvaluatedTime",
                table: "AUC_Orders_WaitingForSending",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EvaluationContent",
                table: "AUC_Orders_WaitingForSending",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EvaluationLevel",
                table: "AUC_Orders_WaitingForSending",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidTime",
                table: "AUC_Orders_WaitingForSending",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedTime",
                table: "AUC_Orders_WaitingForSending",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedTime",
                table: "AUC_Orders_WaitingForReceiving",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EvaluatedTime",
                table: "AUC_Orders_WaitingForReceiving",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EvaluationContent",
                table: "AUC_Orders_WaitingForReceiving",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EvaluationLevel",
                table: "AUC_Orders_WaitingForReceiving",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidTime",
                table: "AUC_Orders_WaitingForReceiving",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentTime",
                table: "AUC_Orders_WaitingForReceiving",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedTime",
                table: "AUC_Orders_WaitingForPayment",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EvaluatedTime",
                table: "AUC_Orders_WaitingForPayment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EvaluationContent",
                table: "AUC_Orders_WaitingForPayment",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EvaluationLevel",
                table: "AUC_Orders_WaitingForPayment",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedTime",
                table: "AUC_Orders_WaitingForPayment",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentTime",
                table: "AUC_Orders_WaitingForPayment",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedTime",
                table: "AUC_Orders_WaitingForEvaluating",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EvaluationContent",
                table: "AUC_Orders_WaitingForEvaluating",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EvaluationLevel",
                table: "AUC_Orders_WaitingForEvaluating",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidTime",
                table: "AUC_Orders_WaitingForEvaluating",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedTime",
                table: "AUC_Orders_WaitingForEvaluating",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentTime",
                table: "AUC_Orders_WaitingForEvaluating",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedTime",
                table: "AUC_Orders_Completed",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<DateTime>(
                name: "EvaluatedTime",
                table: "AUC_Orders_Completed",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidTime",
                table: "AUC_Orders_Completed",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedTime",
                table: "AUC_Orders_Completed",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentTime",
                table: "AUC_Orders_Completed",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedTime",
                table: "AUC_Orders_WaitingForSending");

            migrationBuilder.DropColumn(
                name: "EvaluatedTime",
                table: "AUC_Orders_WaitingForSending");

            migrationBuilder.DropColumn(
                name: "EvaluationContent",
                table: "AUC_Orders_WaitingForSending");

            migrationBuilder.DropColumn(
                name: "EvaluationLevel",
                table: "AUC_Orders_WaitingForSending");

            migrationBuilder.DropColumn(
                name: "PaidTime",
                table: "AUC_Orders_WaitingForSending");

            migrationBuilder.DropColumn(
                name: "ReceivedTime",
                table: "AUC_Orders_WaitingForSending");

            migrationBuilder.DropColumn(
                name: "CompletedTime",
                table: "AUC_Orders_WaitingForReceiving");

            migrationBuilder.DropColumn(
                name: "EvaluatedTime",
                table: "AUC_Orders_WaitingForReceiving");

            migrationBuilder.DropColumn(
                name: "EvaluationContent",
                table: "AUC_Orders_WaitingForReceiving");

            migrationBuilder.DropColumn(
                name: "EvaluationLevel",
                table: "AUC_Orders_WaitingForReceiving");

            migrationBuilder.DropColumn(
                name: "PaidTime",
                table: "AUC_Orders_WaitingForReceiving");

            migrationBuilder.DropColumn(
                name: "SentTime",
                table: "AUC_Orders_WaitingForReceiving");

            migrationBuilder.DropColumn(
                name: "CompletedTime",
                table: "AUC_Orders_WaitingForPayment");

            migrationBuilder.DropColumn(
                name: "EvaluatedTime",
                table: "AUC_Orders_WaitingForPayment");

            migrationBuilder.DropColumn(
                name: "EvaluationContent",
                table: "AUC_Orders_WaitingForPayment");

            migrationBuilder.DropColumn(
                name: "EvaluationLevel",
                table: "AUC_Orders_WaitingForPayment");

            migrationBuilder.DropColumn(
                name: "ReceivedTime",
                table: "AUC_Orders_WaitingForPayment");

            migrationBuilder.DropColumn(
                name: "SentTime",
                table: "AUC_Orders_WaitingForPayment");

            migrationBuilder.DropColumn(
                name: "CompletedTime",
                table: "AUC_Orders_WaitingForEvaluating");

            migrationBuilder.DropColumn(
                name: "EvaluationContent",
                table: "AUC_Orders_WaitingForEvaluating");

            migrationBuilder.DropColumn(
                name: "EvaluationLevel",
                table: "AUC_Orders_WaitingForEvaluating");

            migrationBuilder.DropColumn(
                name: "PaidTime",
                table: "AUC_Orders_WaitingForEvaluating");

            migrationBuilder.DropColumn(
                name: "ReceivedTime",
                table: "AUC_Orders_WaitingForEvaluating");

            migrationBuilder.DropColumn(
                name: "SentTime",
                table: "AUC_Orders_WaitingForEvaluating");

            migrationBuilder.DropColumn(
                name: "EvaluatedTime",
                table: "AUC_Orders_Completed");

            migrationBuilder.DropColumn(
                name: "PaidTime",
                table: "AUC_Orders_Completed");

            migrationBuilder.DropColumn(
                name: "ReceivedTime",
                table: "AUC_Orders_Completed");

            migrationBuilder.DropColumn(
                name: "SentTime",
                table: "AUC_Orders_Completed");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedTime",
                table: "AUC_Orders_Completed",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
