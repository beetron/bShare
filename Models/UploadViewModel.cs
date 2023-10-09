namespace Bshare.Models
{
    public class UploadViewModel
    {
        public string? FileDescription { get; set; }
        public string? ShortLink { get; set; }
        public string? Password { get; set; }
        public byte[]? QrImage { get; set; }
        public DateTime DateUpload { get; set; }
        public DateTime DateExpire { get; set; }
        public virtual ICollection<FileDetail>? FileDetails { get; set; } = new List<FileDetail>();
    }
}
