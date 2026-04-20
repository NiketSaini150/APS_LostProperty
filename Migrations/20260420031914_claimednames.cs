using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APS_LostProperty.Migrations
{
    /// <inheritdoc />
    public partial class claimednames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "Claim",
                newName: "ClaimedItemName");

            migrationBuilder.RenameColumn(
                name: "Description", 
                table: "Claim",
                newName: "ClaimedDescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClaimedItemName",
                table: "Claim",
                newName: "ItemName");

            migrationBuilder.RenameColumn(
                name: "ClaimedDescription",
                table: "Claim",
                newName: "Description");
        }
    }
}
