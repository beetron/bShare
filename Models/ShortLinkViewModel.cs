namespace Bshare.Models
{
    public class ShortLinkViewModel
    {
        public int FileUploadId { get; set; }
        public string? FileDescription { get; set; }
        public string ShortLink { get; set; }
        public string? Password { get; set; }
        public byte[] QrImage { get; set; }
        public DateTime DateExpire { get; set; }
        public virtual ICollection<FileDetail>? FileDetails { get; set; } = new List<FileDetail>();
    }
}
