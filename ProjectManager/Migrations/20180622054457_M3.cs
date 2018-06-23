using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ProjectManager.Migrations
{
    public partial class M3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductKnowledgeRules_ProductKnowledgeVariables_LeftVariableId",
                table: "ProductKnowledgeRules");

            migrationBuilder.DropTable(
                name: "Condition");

            migrationBuilder.DropIndex(
                name: "IX_ProductKnowledgeRules_LeftVariableId",
                table: "ProductKnowledgeRules");

            migrationBuilder.DropColumn(
                name: "LeftVariableId",
                table: "ProductKnowledgeRules");

            migrationBuilder.CreateTable(
                name: "Expression",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LeftVariableId = table.Column<int>(nullable: true),
                    Ratio = table.Column<int>(nullable: false),
                    RightVariable = table.Column<string>(nullable: true),
                    RuleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expression", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expression_ProductKnowledgeVariables_LeftVariableId",
                        column: x => x.LeftVariableId,
                        principalTable: "ProductKnowledgeVariables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expression_ProductKnowledgeRules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "ProductKnowledgeRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expression_LeftVariableId",
                table: "Expression",
                column: "LeftVariableId");

            migrationBuilder.CreateIndex(
                name: "IX_Expression_RuleId",
                table: "Expression",
                column: "RuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expression");

            migrationBuilder.AddColumn<int>(
                name: "LeftVariableId",
                table: "ProductKnowledgeRules",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Condition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductKnowledgeRuleId = table.Column<int>(nullable: true),
                    Ratio = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Condition_ProductKnowledgeRules_ProductKnowledgeRuleId",
                        column: x => x.ProductKnowledgeRuleId,
                        principalTable: "ProductKnowledgeRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductKnowledgeRules_LeftVariableId",
                table: "ProductKnowledgeRules",
                column: "LeftVariableId");

            migrationBuilder.CreateIndex(
                name: "IX_Condition_ProductKnowledgeRuleId",
                table: "Condition",
                column: "ProductKnowledgeRuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductKnowledgeRules_ProductKnowledgeVariables_LeftVariableId",
                table: "ProductKnowledgeRules",
                column: "LeftVariableId",
                principalTable: "ProductKnowledgeVariables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
