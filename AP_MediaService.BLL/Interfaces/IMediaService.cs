using AP_MediaService.Common.Models.ResponseModels;
using AP_MediaService.DTO.Models.RequestModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.BLL.Interfaces
{ 
    public interface IMediaService
    {
        Task<Response> InsertMediaServiceConfigAsync(InsertMediaServiceConfigReq req);
        Task<Response> UploadAsync(IFormFile file, string dirPath, string fileName, string bucket);
        Task<Response> UploadToTempAsync(IFormFile file);
        Task<Response> GetBucketListAsync();
        Task<Response> GetFileAsyncAsync(string bucket, string dirPath, string fileName);
        
    }
}
