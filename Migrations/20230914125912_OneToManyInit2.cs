using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bshare.Migrations
{
    /// <inheritdoc />
    public partial class OneToManyInit2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDetails_FileUploads_FileUploadId",
                table: "FileDetails");

            migrationBuilder.DropIndex(
                name: "IX_FileDetails_FileUploadId",
                table: "FileDetails");

            migrationBuilder.DropColumn(
                name: "FileUploadId",
                table: "FileDetails");

            migrationBuilder.AlterColumn<int>(
                name: "FileDetailId",
                table: "FileDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_FileDetails_FileUploads_FileDetailId",
                table: "FileDetails",
                column: "FileDetailId",
                principalTable: "FileUploads",
                principalColumn: "FileUploadId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDetails_FileUploads_FileDetailId",
                table: "FileDetails");

            migrationBuilder.AlterColumn<int>(
                name: "FileDetailId",
                table: "FileDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "FileUploadId",
                table: "FileDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FileDetails_FileUploadId",
                table: "FileDetails",
                column: "FileUploadId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileDetails_FileUploads_FileUploadId",
                table: "FileDetails",
                column: "FileUploadId",
                principalTable: "FileUploads",
                principalColumn: "FileUploadId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
