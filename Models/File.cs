using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

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
        public byte[]? QrImage { get; set; }
        public DateTime TimeUpload { get; set; }
        public DateTime TimeExpire { get; set; }

    }
}
