using AP_MediaService.BLL.Models;
using AP_MediaService.Common.Models;
using AP_MediaService.DTO.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.BLL.Helper.Interfaces
{
    public interface IFileHelper
    { 
        Task<FileUploadResult> UploadFile(IFormFile file, string? dirPath, string fileName, string bucket);
        Task<List<BucketModel>> GetBucketListAsync();
        Task<string> GetFileUrlAsync(string bucket, string dirPath, string fileName);
    }
}
