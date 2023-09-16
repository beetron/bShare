using System.ComponentModel.DataAnnotations;

namespace Bshare.Models
{
    public class FileUpload
    {
        [Key]
        public int FileUploadId { get; set; }
        public string ShortLink { get; set; }
        public string? Password { get; set; }
        public byte[]? QrImage { get; set; }
        public DateTime DateUpload { get; set; }
        public DateTime DateExpire { get; set; }
        public virtual ICollection<FileDetail> FileDetails { get; set; } = new List<FileDetail>();

    }
}