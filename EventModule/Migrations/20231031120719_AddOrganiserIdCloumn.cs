using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventModule.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganiserIdCloumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganiserId",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganiserId",
                table: "Events");
        }
    }
}
