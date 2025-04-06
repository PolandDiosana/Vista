using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vista_Subdivision.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIDFromFacilityReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "FacilityReservations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "FacilityReservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
