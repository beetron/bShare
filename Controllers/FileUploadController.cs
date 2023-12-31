﻿using System.Drawing;
using System.Drawing.Imaging;
using Bshare.Models;
using Bshare.Repository;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using Bshare.Services;
using QRCoder;

namespace Bshare.Controllers
{
    public class FileUploadController : Controller
    {
        readonly IFilesUploadRepository _iFilesUploadRepository;
        readonly IFileService _iFileService;
        string? _localFilePath = Environment.GetEnvironmentVariable("bshare_UploadLocation");
        string? _bshareLink = Environment.GetEnvironmentVariable("bshare_AppUrl");

        public FileUploadController(IFilesUploadRepository iFilesUploadRepository, IFileService iFileService)
        {
            _iFilesUploadRepository = iFilesUploadRepository;
            _iFileService = iFileService;
        }

        // Create method to create & upload new file
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(510 * 1024 * 1024)] // Total 510mb
        [RequestFormLimits(MultipartBodyLengthLimit = 505 * 1024 * 1024)] // Form data 505mb
        [Route ("/file/create")]
        public async Task <IActionResult> Create(UploadViewModel uploadViewModel, string dropdownSelection, List<IFormFile> files)
        {
                uploadViewModel.DateUpload = DateTime.Now;

                // Set DateExpired from dropdown selection
                switch (dropdownSelection)
                {
                    case "12":
                        uploadViewModel.DateExpire = DateTime.Now.AddHours(12);
                        break;
                    case "24":
                        uploadViewModel.DateExpire = DateTime.Now.AddHours(24);
                        break;
                    case "48":
                        uploadViewModel.DateExpire = DateTime.Now.AddHours(48);
                        break;
                    default:
                        uploadViewModel.DateExpire = DateTime.Now.AddHours(12);
                        break;
                }

                // Generate short link and check database if unique (6 characters specified)
                uploadViewModel.ShortLink = await _iFilesUploadRepository.GenerateShortLinkAsync(6);

                // Generate QrCode
                string qrPayload = _bshareLink + uploadViewModel.ShortLink;
                QRCodeGenerator qrCodeGenerator = new();
                QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(qrPayload, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                byte[] qrImageBytes;

                using (MemoryStream ms = new MemoryStream())
                {
                    qrCodeImage.Save(ms, ImageFormat.Bmp);
                    qrImageBytes = ms.ToArray();
                }

                uploadViewModel.QrImage = qrImageBytes;

                // Create new directory and save files to local storage
                uploadViewModel.FileDetails = await _iFileService.SaveFileAsync(uploadViewModel, files, _localFilePath);
                if (ModelState.IsValid)
                {
                    FileUpload fileUpload = new FileUpload
                    {
                        FileDescription = uploadViewModel.FileDescription,
                        Password = uploadViewModel.Password,
                        ShortLink = uploadViewModel.ShortLink,
                        QrImage = uploadViewModel.QrImage,
                        DateUpload = uploadViewModel.DateUpload,
                        DateExpire = uploadViewModel.DateExpire,
                        FileDetails = uploadViewModel.FileDetails
                    };

                    // Save database tables
                    await _iFilesUploadRepository.CreateFileUploadAsync(fileUpload);

                    //return RedirectToAction(nameof(fileUpload.ShortLink));
                    //return RedirectToAction("", "", new { id = fileUpload.ShortLink });
                    return Redirect($"~/{fileUpload.ShortLink}");
                    //return View("Upload");
                }
                // return View("Upload");
            return View("Upload");
        }

        public IActionResult Upload()
        {
            return View("Upload");
        }

        // Redirects to short link URL if the short link exists in database
        [Route("~/{shortLink}")]
        public async Task<IActionResult> ShortLink(string shortLink)
        {
            bool shortLinkExists = await _iFilesUploadRepository.CheckShortLinkAsync(shortLink);
            if (shortLinkExists)
            {
                FileUpload fileRecord = await _iFilesUploadRepository.GetByShortLinkAsync(shortLink);
                fileRecord.ShortLink = _bshareLink + fileRecord.ShortLink;
                ShortLinkViewModel shortLinkViewModel = new ShortLinkViewModel
                {
                    FileUploadId = fileRecord.FileUploadId,
                    FileDescription = fileRecord.FileDescription,
                    Password = fileRecord.Password,
                    ShortLink = fileRecord.ShortLink,
                    QrImage = fileRecord.QrImage,
                    DateExpire = fileRecord.DateExpire,
                    FileDetails = fileRecord.FileDetails
                };
                return View(shortLinkViewModel);
                // return View(fileRecord);
            }
            else
            {
                return View("BadLink");
            }
        }

        public IActionResult BadLink()
        {
            return View();
        }

        // Download single file, or multiple as Zip
        [Route("/file/Download")]
        public IActionResult DownloadFile(ShortLinkViewModel shortLinkViewModel, string[] fileNames, string fileName)
        {
            if (!ModelState.IsValid)
            {
                // Handle ModelState errors here or log them for debugging
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    // Log or handle the error as needed
                }

                // You can also choose to return a view or an error message here
                // Example: return View("ErrorViewName");
            }

            string fileLocation = Path.Combine(_localFilePath + shortLinkViewModel.ShortLink);
            string fileNameZip = shortLinkViewModel.ShortLink + ".zip";

            // Multiple file download as Zip
            if (fileNames.Length >= 2)
            {
                string fileDestination = Path.Combine(_localFilePath, fileNameZip);

                // Create zip file
                if (!System.IO.File.Exists(fileDestination))
                {
                    ZipFile.CreateFromDirectory(fileLocation, fileDestination);
                }

                using MemoryStream memoryStream = new MemoryStream();
                using (FileStream fileStream = new FileStream(fileDestination, FileMode.Open))
                {
                    fileStream.CopyTo(memoryStream);
                }

                // Save into memory stream
                var fileResult = File(memoryStream.ToArray(), "application/zip", fileNameZip);

                // Remove zip file
                System.IO.File.Delete(fileDestination);

                // Return stored zip from memory stream
                return fileResult;

            }

            // Single file download
            if (!String.IsNullOrEmpty(fileName))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (FileStream fileStream = new FileStream(fileLocation + "/" + fileName, FileMode.Open))
                    {
                        fileStream.CopyTo(memoryStream);
                    }

                    // Use a FileResult with a specified content disposition header
                    // var contentDisposition = new System.Net.Mime.ContentDisposition
                    // {
                    //     FileName = fileNameSingle,
                    //     Inline = false,  // Set to true if you want the browser to attempt to display the file inline
                    // };
                    // Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
                    // return File(memoryStream.ToArray(), "image/*");

                    return File(memoryStream.ToArray(), "image/*", fileName);
                }
            }
            return Ok();
        }

        // Delete Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/file/Delete")]
        public async Task<IActionResult> DeleteUpload(DeleteViewModel deleteViewModel)
        {
            FileUpload fileUpload = new FileUpload
            {
                FileUploadId = deleteViewModel.FileUploadId,
                ShortLink = deleteViewModel.ShortLink,
                Password = deleteViewModel.Password
            };

            // Check if password is correct
            if (await _iFilesUploadRepository.CheckPasswordAsync(fileUpload, fileUpload.Password))
            {
                // Delete physical files on server
                await _iFileService.DeleteFile(deleteViewModel, _localFilePath);

                // Delete file upload record from database
                await _iFilesUploadRepository.DeleteAsync(fileUpload.FileUploadId);

                return Redirect("~/");
                // return View("Upload");
            }

            TempData["message"] = "Wrong password";
            // return Redirect($"/{fileUpload.ShortLink}");
            return Redirect($"~/{fileUpload.ShortLink}");
        }
    }
}
