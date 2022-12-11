using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenCreated",
                schema: "Membership",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpires",
                schema: "Membership",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TokenCreated",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TokenExpires",
                schema: "Membership",
                table: "Users");
        }
    }
}
