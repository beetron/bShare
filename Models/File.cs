using System.ComponentModel.DataAnnotations;

namespace Bshare.Models
{
    public class File
    {
        [Key]
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FileTitle { get; set; }
        public string? FileDescription { get; set; }
        public string? FilePath { get; set; }
        public string? FileShortLink { get; set; }
        public DateTime UploadTime { get; set; }
        public DateTime ExpireTime { get; set; }

    }
}
