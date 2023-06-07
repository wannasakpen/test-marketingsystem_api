using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.DTO.Models.RequestModels
{ 
    public partial class InsertMediaServiceConfigReq
    {
        public string MediaServerName { get; set; }
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string? PublicURL { get; set; }
        public string DefaultBucket { get; set; }
        public string TempBucket { get; set; }
        public bool? WithSSL { get; set; }
        public Guid StorageTypeID { get; set; }
    }

    public partial class UploadFileReq
    {
        public IFormFile file { get; set; }
        public string dirPath { get; set; }
        public string fileName { get; set; }
        public string bucket { get; set; } 
    }
}
