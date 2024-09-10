using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aurigma.DirectMail.Sample.DAL.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class RenameRecipientColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QRCoreUrl",
                table: "Recipient",
                newName: "QRCodeUrl"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QRCodeUrl",
                table: "Recipient",
                newName: "QRCoreUrl"
            );
        }
    }
}
