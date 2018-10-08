using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Kuaiyipai.Migrations
{
    public partial class AddSpecialActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvitationCode",
                table: "AUC_Items_Terminated",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SpecialActivityId",
                table: "AUC_Items_Terminated",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvitationCode",
                table: "AUC_Items_Drafting",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SpecialActivityId",
                table: "AUC_Items_Drafting",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvitationCode",
                table: "AUC_Items_Completed",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SpecialActivityId",
                table: "AUC_Items_Completed",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvitationCode",
                table: "AUC_Items_Auctioning",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SpecialActivityId",
                table: "AUC_Items_Auctioning",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AUC_SpecialActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CoverUrl = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: false),
                    InvitationCode = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUC_SpecialActivities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUC_SpecialActivities");

            migrationBuilder.DropColumn(
                name: "InvitationCode",
                table: "AUC_Items_Terminated");

            migrationBuilder.DropColumn(
                name: "SpecialActivityId",
                table: "AUC_Items_Terminated");

            migrationBuilder.DropColumn(
                name: "InvitationCode",
                table: "AUC_Items_Drafting");

            migrationBuilder.DropColumn(
                name: "SpecialActivityId",
                table: "AUC_Items_Drafting");

            migrationBuilder.DropColumn(
                name: "InvitationCode",
                table: "AUC_Items_Completed");

            migrationBuilder.DropColumn(
                name: "SpecialActivityId",
                table: "AUC_Items_Completed");

            migrationBuilder.DropColumn(
                name: "InvitationCode",
                table: "AUC_Items_Auctioning");

            migrationBuilder.DropColumn(
                name: "SpecialActivityId",
                table: "AUC_Items_Auctioning");
        }
    }
}
