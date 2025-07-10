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
            migrationBuilder.DropColumn(
                name: "PerformedAt",
                table: "ClaimWorkflowSteps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PerformedAt",
                table: "ClaimWorkflowSteps",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
