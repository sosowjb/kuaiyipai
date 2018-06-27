using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Kuaiyipai.Migrations
{
    public partial class AddOrderEvaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Evaluation",
                table: "AUC_Orders_WaitingForEvaluating",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Evaluation",
                table: "AUC_Orders_WaitingForEvaluating");
        }
    }
}
