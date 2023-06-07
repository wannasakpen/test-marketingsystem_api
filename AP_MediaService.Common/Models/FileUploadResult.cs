using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Models
{
    public class FileUploadResult
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? BucketName { get; set; }
    }

    public class UploadFileModel
    {
        public string? OriginalName { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? BucketName { get; set; }
        public bool IsTemp { get; set; }
    }
}
