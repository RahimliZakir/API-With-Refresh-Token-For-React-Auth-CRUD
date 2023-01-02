using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class DeletedDateLogicAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Trucks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Cars",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Buses",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Buses");
        }
    }
}
