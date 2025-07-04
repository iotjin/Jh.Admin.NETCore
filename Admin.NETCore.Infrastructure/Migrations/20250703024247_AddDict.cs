using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Admin.NETCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "UserRole",
                newName: "AssignStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssignStatus",
                table: "UserRole",
                newName: "Status");
        }
    }
}
