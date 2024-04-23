using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffNook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_name_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDateDate",
                table: "Projects",
                newName: "EndDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Projects",
                newName: "EndDateDate");
        }
    }
}
