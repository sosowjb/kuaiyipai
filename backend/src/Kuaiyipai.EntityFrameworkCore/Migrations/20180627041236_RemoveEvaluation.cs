using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Kuaiyipai.Migrations
{
    public partial class RemoveEvaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Evaluation",
                table: "AUC_Orders_WaitingForEvaluating");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Evaluation",
                table: "AUC_Orders_WaitingForEvaluating",
                nullable: true);
        }
    }
}
