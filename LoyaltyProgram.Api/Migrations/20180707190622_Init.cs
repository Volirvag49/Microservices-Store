using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LoyaltyProgram.Api.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoyaltyProgramUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LoyaltyPoints = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyProgramUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyProgramSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Interests = table.Column<string>(nullable: true),
                    LoyaltyProgramUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyProgramSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoyaltyProgramSettings_LoyaltyProgramUser_LoyaltyProgramUserId",
                        column: x => x.LoyaltyProgramUserId,
                        principalTable: "LoyaltyProgramUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyProgramSettings_LoyaltyProgramUserId",
                table: "LoyaltyProgramSettings",
                column: "LoyaltyProgramUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoyaltyProgramSettings");

            migrationBuilder.DropTable(
                name: "LoyaltyProgramUser");
        }
    }
}
