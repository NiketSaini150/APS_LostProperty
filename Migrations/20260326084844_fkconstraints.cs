using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APS_LostProperty.Migrations
{
    /// <inheritdoc />
    public partial class fkconstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claim_LostItem_MatchedLostItemID",
                table: "Claim");

            migrationBuilder.AddForeignKey(
                name: "FK_Claim_LostItem_MatchedLostItemID",
                table: "Claim",
                column: "MatchedLostItemID",
                principalTable: "LostItem",
                principalColumn: "LostItemID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claim_LostItem_MatchedLostItemID",
                table: "Claim");

            migrationBuilder.AddForeignKey(
                name: "FK_Claim_LostItem_MatchedLostItemID",
                table: "Claim",
                column: "MatchedLostItemID",
                principalTable: "LostItem",
                principalColumn: "LostItemID");
        }
    }
}
