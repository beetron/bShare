﻿// <auto-generated />
using System;
using Bshare.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bshare.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230918121318_FileUploadModel-PropRename2")]
    partial class FileUploadModelPropRename2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Bshare.Models.FileDetail", b =>
                {
                    b.Property<int>("FileDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FileSize")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("FileUploadId")
                        .HasColumnType("int");

                    b.HasKey("FileDetailId");

                    b.HasIndex("FileUploadId");

                    b.ToTable("FileDetails");
                });

            modelBuilder.Entity("Bshare.Models.FileUpload", b =>
                {
                    b.Property<int>("FileUploadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateExpire")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateUpload")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FileDescription")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("QrImage")
                        .HasColumnType("longblob");

                    b.Property<string>("ShortLink")
                        .HasColumnType("longtext");

                    b.HasKey("FileUploadId");

                    b.ToTable("FileUploads");
                });

            modelBuilder.Entity("Bshare.Models.FileDetail", b =>
                {
                    b.HasOne("Bshare.Models.FileUpload", "FileUpload")
                        .WithMany("FileDetails")
                        .HasForeignKey("FileUploadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FileUpload");
                });

            modelBuilder.Entity("Bshare.Models.FileUpload", b =>
                {
                    b.Navigation("FileDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
