using System.ComponentModel.DataAnnotations;

namespace Bshare.Models
{
    public class FileUpload
    {
        [Key]
        public int FileUploadId { get; set; }
        public string? ShortLink { get; set; }
        public byte[]? QrImage { get; set; }
        public DateTime TimeUpload { get; set; }
        public DateTime TimeExpire { get; set; }
        public ICollection<FileDetail> FileDetails { get; set; }


    }
}