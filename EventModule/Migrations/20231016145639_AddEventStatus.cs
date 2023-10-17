using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventModule.Migrations
{
    /// <inheritdoc />
    public partial class AddEventStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventStatus",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventStatus",
                table: "Events");
        }
    }
}
