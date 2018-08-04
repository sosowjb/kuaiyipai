using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Kuaiyipai.Migrations
{
    public partial class DeleteOrderForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AUC_Orders_Completed_AUC_Addresses_AddressId",
                table: "AUC_Orders_Completed");

            migrationBuilder.DropForeignKey(
                name: "FK_AUC_Orders_WaitingForEvaluating_AUC_Addresses_AddressId",
                table: "AUC_Orders_WaitingForEvaluating");

            migrationBuilder.DropForeignKey(
                name: "FK_AUC_Orders_WaitingForPayment_AUC_Addresses_AddressId",
                table: "AUC_Orders_WaitingForPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_AUC_Orders_WaitingForReceiving_AUC_Addresses_AddressId",
                table: "AUC_Orders_WaitingForReceiving");

            migrationBuilder.DropForeignKey(
                name: "FK_AUC_Orders_WaitingForSending_AUC_Addresses_AddressId",
                table: "AUC_Orders_WaitingForSending");

            migrationBuilder.DropIndex(
                name: "IX_AUC_Orders_WaitingForSending_AddressId",
                table: "AUC_Orders_WaitingForSending");

            migrationBuilder.DropIndex(
                name: "IX_AUC_Orders_WaitingForReceiving_AddressId",
                table: "AUC_Orders_WaitingForReceiving");

            migrationBuilder.DropIndex(
                name: "IX_AUC_Orders_WaitingForPayment_AddressId",
                table: "AUC_Orders_WaitingForPayment");

            migrationBuilder.DropIndex(
                name: "IX_AUC_Orders_WaitingForEvaluating_AddressId",
                table: "AUC_Orders_WaitingForEvaluating");

            migrationBuilder.DropIndex(
                name: "IX_AUC_Orders_Completed_AddressId",
                table: "AUC_Orders_Completed");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "AUC_Orders_WaitingForSending",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "AUC_Orders_WaitingForReceiving",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "AUC_Orders_WaitingForPayment",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "AUC_Orders_WaitingForEvaluating",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "AUC_Orders_Completed",
                nullable: true,
                oldClrType: typeof(Guid));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "AUC_Orders_WaitingForSending",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "AUC_Orders_WaitingForReceiving",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "AUC_Orders_WaitingForPayment",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "AUC_Orders_WaitingForEvaluating",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "AUC_Orders_Completed",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AUC_Orders_WaitingForSending_AddressId",
                table: "AUC_Orders_WaitingForSending",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AUC_Orders_WaitingForReceiving_AddressId",
                table: "AUC_Orders_WaitingForReceiving",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AUC_Orders_WaitingForPayment_AddressId",
                table: "AUC_Orders_WaitingForPayment",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AUC_Orders_WaitingForEvaluating_AddressId",
                table: "AUC_Orders_WaitingForEvaluating",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AUC_Orders_Completed_AddressId",
                table: "AUC_Orders_Completed",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AUC_Orders_Completed_AUC_Addresses_AddressId",
                table: "AUC_Orders_Completed",
                column: "AddressId",
                principalTable: "AUC_Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AUC_Orders_WaitingForEvaluating_AUC_Addresses_AddressId",
                table: "AUC_Orders_WaitingForEvaluating",
                column: "AddressId",
                principalTable: "AUC_Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AUC_Orders_WaitingForPayment_AUC_Addresses_AddressId",
                table: "AUC_Orders_WaitingForPayment",
                column: "AddressId",
                principalTable: "AUC_Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AUC_Orders_WaitingForReceiving_AUC_Addresses_AddressId",
                table: "AUC_Orders_WaitingForReceiving",
                column: "AddressId",
                principalTable: "AUC_Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AUC_Orders_WaitingForSending_AUC_Addresses_AddressId",
                table: "AUC_Orders_WaitingForSending",
                column: "AddressId",
                principalTable: "AUC_Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
