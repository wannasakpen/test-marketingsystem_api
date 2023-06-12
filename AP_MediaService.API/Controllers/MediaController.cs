using AP_MediaService.Common.Helper.Interface;
using AP_MediaService.Common.Helper.Logging;
using AP_MediaService.Common.Models.ResponseModels;
using AP_MediaService.Common.Utilities;
using Microsoft.AspNetCore.Mvc;
using AP_MediaService.Common.MappingErrors;
using AP_MediaService.BLL.Interfaces;
using AP_MediaService.DTO.Models.ResponseModels;
using AP_MediaService.DTO.Models.RequestModels;
using Microsoft.VisualBasic;
using AP_MediaService.Common.ActionFilters;
using AP_MediaService.Common.Models;



namespace AP_MediaService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        private readonly IHttpResultHelper _httpResultHelper;
        LogModel logModel = null;
        public MediaController(IMediaService mediaService, IHttpResultHelper httpResultHelper)
        {
            ControllerHeader.setControllerName("Media");
            _mediaService = mediaService;
            _httpResultHelper = httpResultHelper;
            logModel = new LogModel();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("InsertMediaServiceConfig")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<ActionResult> InsertMediaServiceConfigAsync(InsertMediaServiceConfigReq req)
        {
            var result = await _mediaService.InsertMediaServiceConfigAsync(req);
            return await _httpResultHelper.CustomResult(result.RespCode.GetHttpStatusCode(), result, logModel); 
        }
      
        [HttpGet]
        [Route("GetBucketListAsync")]
        [ProducesResponseType(typeof(Response<FileResponse>), 200)]
        [SwaggerHeader(HeaderParameter.AccessKey, "MediaServerKey", "", true)]
        public async Task<ActionResult> GetBucketListAsync([FromHeader(Name = HeaderParameter.AccessKey)] string AccessKey = null)
        {
            var result = await _mediaService.GetBucketListAsync();
            return await _httpResultHelper.CustomResult(result.RespCode.GetHttpStatusCode(), result, logModel);
        }

        [HttpGet]
        [Route("GetFileAsync")]
        [ProducesResponseType(typeof(Response<FileResponse>), 200)]
        [SwaggerHeader(HeaderParameter.AccessKey, "MediaServerKey", "", true)]
        public async Task<ActionResult> GetFileAsyncAsync(string bucket, string? dirPath, string fileName, [FromHeader(Name = HeaderParameter.AccessKey)] string AccessKey = null)
        {
            var result = await _mediaService.GetFileAsyncAsync(bucket, dirPath, fileName);
            return await _httpResultHelper.CustomResult(result.RespCode.GetHttpStatusCode(), result, logModel);
        }

        [HttpPost]
        [Route("Upload")]
        [ProducesResponseType(typeof(Response<FileResponse>), 200)]
        [SwaggerHeader(HeaderParameter.AccessKey, "MediaServerKey", "", true)]
        public async Task<ActionResult> UploadFileAsync([FromForm] UploadFileReq req, [FromHeader(Name = HeaderParameter.AccessKey)] string AccessKey = null)
        {
            var result = await _mediaService.UploadAsync(req.file, req.dirPath, req.fileName, req.bucket);
            return await _httpResultHelper.CustomResult(result.RespCode.GetHttpStatusCode(), result, logModel);
        }

        [HttpPost]
        [Route("Upload/Temp")]
        [ProducesResponseType(typeof(Response<FileResponse>), 200)]
        [SwaggerHeader(HeaderParameter.AccessKey, "MediaServerKey", "", true)]
        public async Task<ActionResult> UploadToTempAsync(IFormFile file, [FromHeader(Name = HeaderParameter.AccessKey)] string AccessKey = null)
        {
            var result = await _mediaService.UploadToTempAsync(file);
            return await _httpResultHelper.CustomResult(result.RespCode.GetHttpStatusCode(), result, logModel);
        }
    }
}
