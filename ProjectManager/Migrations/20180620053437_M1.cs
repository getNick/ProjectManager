using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ProjectManager.Migrations
{
    public partial class M1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerWish_AdditionalFiles_AdditionalFilesId",
                table: "CustomerWish");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerWish_Participants_AutorId",
                table: "CustomerWish");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerWish_Projects_ProjectId",
                table: "CustomerWish");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerWish",
                table: "CustomerWish");

            migrationBuilder.RenameTable(
                name: "CustomerWish",
                newName: "CustomerWishes");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerWish_ProjectId",
                table: "CustomerWishes",
                newName: "IX_CustomerWishes_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerWish_AutorId",
                table: "CustomerWishes",
                newName: "IX_CustomerWishes_AutorId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerWish_AdditionalFilesId",
                table: "CustomerWishes",
                newName: "IX_CustomerWishes_AdditionalFilesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerWishes",
                table: "CustomerWishes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GanttLinks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SourceTaskId = table.Column<int>(nullable: false),
                    TargetTaskId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GanttLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GanttTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Duration = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Progress = table.Column<decimal>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GanttTasks", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerWishes_AdditionalFiles_AdditionalFilesId",
                table: "CustomerWishes",
                column: "AdditionalFilesId",
                principalTable: "AdditionalFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerWishes_Participants_AutorId",
                table: "CustomerWishes",
                column: "AutorId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerWishes_Projects_ProjectId",
                table: "CustomerWishes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerWishes_AdditionalFiles_AdditionalFilesId",
                table: "CustomerWishes");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerWishes_Participants_AutorId",
                table: "CustomerWishes");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerWishes_Projects_ProjectId",
                table: "CustomerWishes");

            migrationBuilder.DropTable(
                name: "GanttLinks");

            migrationBuilder.DropTable(
                name: "GanttTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerWishes",
                table: "CustomerWishes");

            migrationBuilder.RenameTable(
                name: "CustomerWishes",
                newName: "CustomerWish");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerWishes_ProjectId",
                table: "CustomerWish",
                newName: "IX_CustomerWish_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerWishes_AutorId",
                table: "CustomerWish",
                newName: "IX_CustomerWish_AutorId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerWishes_AdditionalFilesId",
                table: "CustomerWish",
                newName: "IX_CustomerWish_AdditionalFilesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerWish",
                table: "CustomerWish",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerWish_AdditionalFiles_AdditionalFilesId",
                table: "CustomerWish",
                column: "AdditionalFilesId",
                principalTable: "AdditionalFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerWish_Participants_AutorId",
                table: "CustomerWish",
                column: "AutorId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerWish_Projects_ProjectId",
                table: "CustomerWish",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
