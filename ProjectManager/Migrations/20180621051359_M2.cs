using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ProjectManager.Migrations
{
    public partial class M2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductKnowledgeVariables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductKnowledgeVariables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductKnowledgeRules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LeftVariableId = table.Column<int>(nullable: true),
                    Result = table.Column<string>(nullable: true),
                    RightVariableId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductKnowledgeRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductKnowledgeRules_ProductKnowledgeVariables_LeftVariableId",
                        column: x => x.LeftVariableId,
                        principalTable: "ProductKnowledgeVariables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductKnowledgeRules_ProductKnowledgeVariables_RightVariableId",
                        column: x => x.RightVariableId,
                        principalTable: "ProductKnowledgeVariables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_Condition_ProductKnowledgeRuleId",
                table: "Condition",
                column: "ProductKnowledgeRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKnowledgeRules_LeftVariableId",
                table: "ProductKnowledgeRules",
                column: "LeftVariableId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductKnowledgeRules_RightVariableId",
                table: "ProductKnowledgeRules",
                column: "RightVariableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Condition");

            migrationBuilder.DropTable(
                name: "ProductKnowledgeRules");

            migrationBuilder.DropTable(
                name: "ProductKnowledgeVariables");
        }
    }
}
