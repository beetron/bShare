using System.ComponentModel.DataAnnotations;

namespace Bshare.Models
{
    public class FileMain
    {
        [Key]
        public int Id { get; set; }
        public List<string>? FileName { get; set; } = new List<string>();
        public string? FileDescription { get; set; }
        public List<string>? FilePath { get; set; } = new List<string>();
        public string? FileShortLink { get; set; }
        public byte[]? QrImage { get; set; }
        public DateTime TimeUpload { get; set; }
        public DateTime TimeExpire { get; set; }
        


    }
}
