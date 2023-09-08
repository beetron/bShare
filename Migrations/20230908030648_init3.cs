using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bshare.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UploadTime",
                table: "Files",
                newName: "TimeUpload");

            migrationBuilder.RenameColumn(
                name: "ExpireTime",
                table: "Files",
                newName: "TimeExpire");

            migrationBuilder.AddColumn<byte[]>(
                name: "QrImage",
                table: "Files",
                type: "longblob",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrImage",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "TimeUpload",
                table: "Files",
                newName: "UploadTime");

            migrationBuilder.RenameColumn(
                name: "TimeExpire",
                table: "Files",
                newName: "ExpireTime");
        }
    }
}
