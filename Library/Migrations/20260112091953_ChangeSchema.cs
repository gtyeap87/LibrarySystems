using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "lib");

            migrationBuilder.RenameTable(
                name: "Members",
                newName: "Members",
                newSchema: "lib");

            migrationBuilder.RenameTable(
                name: "LoanBooks",
                newName: "LoanBooks",
                newSchema: "lib");

            migrationBuilder.RenameTable(
                name: "Libraries",
                newName: "Libraries",
                newSchema: "lib");

            migrationBuilder.RenameTable(
                name: "BookStocks",
                newName: "BookStocks",
                newSchema: "lib");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "Books",
                newSchema: "lib");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Members",
                schema: "lib",
                newName: "Members");

            migrationBuilder.RenameTable(
                name: "LoanBooks",
                schema: "lib",
                newName: "LoanBooks");

            migrationBuilder.RenameTable(
                name: "Libraries",
                schema: "lib",
                newName: "Libraries");

            migrationBuilder.RenameTable(
                name: "BookStocks",
                schema: "lib",
                newName: "BookStocks");

            migrationBuilder.RenameTable(
                name: "Books",
                schema: "lib",
                newName: "Books");
        }
    }
}
