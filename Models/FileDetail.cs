namespace Bshare.Models
{
    public class FileDetail
    {

        public int FileId { get; set; }
        public FileUpload FileUpload { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }
    }
}
