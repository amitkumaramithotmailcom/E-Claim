using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EClaim.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimDocuments_UserClaim_UserClaimId",
                table: "ClaimDocuments");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropColumn(
                name: "ClaimId",
                table: "ClaimDocuments");

            migrationBuilder.RenameColumn(
                name: "UserClaimId",
                table: "ClaimDocuments",
                newName: "ClaimRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_ClaimDocuments_UserClaimId",
                table: "ClaimDocuments",
                newName: "IX_ClaimDocuments_ClaimRequestId");

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimWorkflowSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<int>(type: "int", nullable: false),
                    PerformedBy = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClaimRequestId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimWorkflowSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimWorkflowSteps_Claims_ClaimRequestId",
                        column: x => x.ClaimRequestId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Claims_UserId",
                table: "Claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimWorkflowSteps_ClaimRequestId",
                table: "ClaimWorkflowSteps",
                column: "ClaimRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimDocuments_Claims_ClaimRequestId",
                table: "ClaimDocuments",
                column: "ClaimRequestId",
                principalTable: "Claims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimDocuments_Claims_ClaimRequestId",
                table: "ClaimDocuments");

            migrationBuilder.DropTable(
                name: "ClaimWorkflowSteps");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.RenameColumn(
                name: "ClaimRequestId",
                table: "ClaimDocuments",
                newName: "UserClaimId");

            migrationBuilder.RenameIndex(
                name: "IX_ClaimDocuments_ClaimRequestId",
                table: "ClaimDocuments",
                newName: "IX_ClaimDocuments_UserClaimId");

            migrationBuilder.AddColumn<int>(
                name: "ClaimId",
                table: "ClaimDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimDocuments_UserClaim_UserClaimId",
                table: "ClaimDocuments",
                column: "UserClaimId",
                principalTable: "UserClaim",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
