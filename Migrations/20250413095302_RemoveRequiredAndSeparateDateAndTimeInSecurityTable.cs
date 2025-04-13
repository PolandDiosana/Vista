using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vista_Subdivision.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRequiredAndSeparateDateAndTimeInSecurityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "EntryTime",
                table: "Securities",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EntryDate",
                table: "Securities",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExitDate",
                table: "Securities",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntryDate",
                table: "Securities");

            migrationBuilder.DropColumn(
                name: "ExitDate",
                table: "Securities");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EntryTime",
                table: "Securities",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");
        }
    }
}
