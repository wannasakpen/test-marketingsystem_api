using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.DTO.Models.ResponseModels
{
    public class FileResponse
    {
        public string BucketName { get; set; }
        public string Url { get; set; } 
        public string Name { get; set; } 
        public bool IsTemp { get; set; }
    }
}
