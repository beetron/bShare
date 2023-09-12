using System.ComponentModel.DataAnnotations;

namespace Bshare.Models
{
    public class FileUpload
    {
        [Key]
        public int UploadId { get; set; }
        public ICollection<FileDetail> FileDetail { get; set; }
        public string? ShortLink { get; set; }
        public byte[]? QrImage { get; set; }
        public DateTime TimeUpload { get; set; }
        public DateTime TimeExpire { get; set; }

    }
}
