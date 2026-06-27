using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace learneropsLms.Migrations
{
    /// <inheritdoc />
    public partial class AddEvidenceIsArchived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "EvidenceItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "EvidenceItems");
        }
    }
}
