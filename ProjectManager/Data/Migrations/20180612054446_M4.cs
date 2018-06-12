using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ProjectManager.Data.Migrations
{
    public partial class M4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilePath_AdditionalFiles_OwnerId",
                table: "FilePath");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Projects_ProjectId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_ProjectId",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FilePath",
                table: "FilePath");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Teams");

            migrationBuilder.RenameTable(
                name: "FilePath",
                newName: "FilePaths");

            migrationBuilder.RenameIndex(
                name: "IX_FilePath_OwnerId",
                table: "FilePaths",
                newName: "IX_FilePaths_OwnerId");

            migrationBuilder.AddColumn<double>(
                name: "Budget",
                table: "Projects",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Projects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectLeadId",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Participants",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Departments",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilePaths",
                table: "FilePaths",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectLeadId",
                table: "Projects",
                column: "ProjectLeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_ProjectId",
                table: "Participants",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ProjectId",
                table: "Departments",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Projects_ProjectId",
                table: "Departments",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FilePaths_AdditionalFiles_OwnerId",
                table: "FilePaths",
                column: "OwnerId",
                principalTable: "AdditionalFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Projects_ProjectId",
                table: "Participants",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Participants_ProjectLeadId",
                table: "Projects",
                column: "ProjectLeadId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Projects_ProjectId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_FilePaths_AdditionalFiles_OwnerId",
                table: "FilePaths");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Projects_ProjectId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Participants_ProjectLeadId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectLeadId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Participants_ProjectId",
                table: "Participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FilePaths",
                table: "FilePaths");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ProjectId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Budget",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectLeadId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Departments");

            migrationBuilder.RenameTable(
                name: "FilePaths",
                newName: "FilePath");

            migrationBuilder.RenameIndex(
                name: "IX_FilePaths_OwnerId",
                table: "FilePath",
                newName: "IX_FilePath_OwnerId");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilePath",
                table: "FilePath",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ProjectId",
                table: "Teams",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilePath_AdditionalFiles_OwnerId",
                table: "FilePath",
                column: "OwnerId",
                principalTable: "AdditionalFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Projects_ProjectId",
                table: "Teams",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
