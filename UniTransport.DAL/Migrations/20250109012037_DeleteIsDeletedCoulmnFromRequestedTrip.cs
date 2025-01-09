using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniTransport.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeleteIsDeletedCoulmnFromRequestedTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RequestedTrips");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RequestedTrips",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
