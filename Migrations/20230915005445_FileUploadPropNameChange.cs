using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bshare.Migrations
{
    /// <inheritdoc />
    public partial class FileUploadPropNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeUpload",
                table: "FileUploads",
                newName: "DateUpload");

            migrationBuilder.RenameColumn(
                name: "TimeExpire",
                table: "FileUploads",
                newName: "DateExpire");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateUpload",
                table: "FileUploads",
                newName: "TimeUpload");

            migrationBuilder.RenameColumn(
                name: "DateExpire",
                table: "FileUploads",
                newName: "TimeExpire");
        }
    }
}
