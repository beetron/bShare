namespace Bshare.Models
{
    public class File
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FileTitle { get; set; }
        public string? FileDescription { get; set; }
        public string? FilePath { get; set; }
        public string? FileShortLink { get; set; }
        DateTime UploadTime { get; set; }
        DateTime ExpireTime { get; set; }

    }
}
