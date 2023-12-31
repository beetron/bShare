﻿using System.ComponentModel.DataAnnotations;

namespace Bshare.Models
{
    public class FileDetail
    {
        [Key]
        public int FileDetailId { get; set; }
        public string FileName { get; set; }
        public double FileSize { get; set; }
        public string FilePath { get; set; }
        public int FileUploadId { get; set; }
        public FileUpload FileUpload { get; set; }
    }
}
