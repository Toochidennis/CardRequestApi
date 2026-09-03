using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardRequestApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueReferenceConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CardRequests_ReferenceNumber",
                table: "CardRequests",
                column: "ReferenceNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CardRequests_ReferenceNumber",
                table: "CardRequests");
        }
    }
}
