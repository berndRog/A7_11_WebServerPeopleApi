using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApi.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "remoteImageUrl",
                table: "People",
                newName: "remoteImage");

            migrationBuilder.RenameColumn(
                name: "localImagePath",
                table: "People",
                newName: "localImage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "remoteImage",
                table: "People",
                newName: "remoteImageUrl");

            migrationBuilder.RenameColumn(
                name: "localImage",
                table: "People",
                newName: "localImagePath");
        }
    }
}
