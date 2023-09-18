﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bshare.Migrations
{
    /// <inheritdoc />
    public partial class FileUploadModelPropRename2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileDescription",
                table: "FileUploads",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FileUploads",
                keyColumn: "FileDescription",
                keyValue: null,
                column: "FileDescription",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "FileDescription",
                table: "FileUploads",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
