using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindHeal.Migrations
{
    public partial class migrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssuesTherapist");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IssuesTherapist",
                columns: table => new
                {
                    IssuesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TherapistId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuesTherapist", x => new { x.IssuesId, x.TherapistId });
                    table.ForeignKey(
                        name: "FK_IssuesTherapist_Issues_IssuesId",
                        column: x => x.IssuesId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssuesTherapist_Therapists_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "Therapists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_IssuesTherapist_TherapistId",
                table: "IssuesTherapist",
                column: "TherapistId");
        }
    }
}
