
using AP_MediaService.BLL.Helper;
using AP_MediaService.BLL.Helper.Interfaces;
using AP_MediaService.BLL.Interfaces;
using AP_MediaService.BLL.Models;
using AP_MediaService.Common.Helper.Logging;
using AP_MediaService.Common.Interfaces;
using AP_MediaService.Common.MappingErrors;
using AP_MediaService.Common.Models.ResponseModels;
using AP_MediaService.DAL.Models.GenerateModels;
using AP_MediaService.DAL.Models.MasterKey;
using AP_MediaService.DAL.Repositories.IRepositories;
using AP_MediaService.DTO.Models.RequestModels;
using AP_MediaService.DTO.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.BLL.Services
{ 
    public class MediaService : IMediaService
    {
        private readonly IHeadersUtils _headersUtil;
        private readonly ILogger _logger;
        private readonly IMediaRepository _mediaRepository;
        private MSTMediaServerConfig _mediaConfig = null;

        public MediaService(ILogger<MediaService> logger, IHeadersUtils headersUtil, IMediaRepository mediaRepository)
        {
            _logger = logger;
            _headersUtil = headersUtil;
            _mediaRepository = mediaRepository;
        }

        public async Task<Response> InsertMediaServiceConfigAsync(InsertMediaServiceConfigReq req)
        {
            Response result = null;
            try
            {
                var MediaServerConfigID = Guid.NewGuid();
                MSTMediaServerConfig Data = new MSTMediaServerConfig()
                {
                    MediaServerConfigID = MediaServerConfigID,
                    MediaServerName = req.MediaServerName,
                    MediaServerKey = MediaServerConfigID.ToString().Replace("-", ""),
                    Endpoint = req.Endpoint,
                    AccessKey = req.AccessKey,
                    SecretKey = req.SecretKey,
                    PublicURL = req.PublicURL,
                    DefaultBucket = req.DefaultBucket,
                    TempBucket = req.TempBucket,
                    WithSSL = req.WithSSL,
                    StorageTypeID = req.StorageTypeID,
                    CreatedByUserID = null,
                    CreatedDate = DateTime.Now,
                    
                };

                var Resp = await _mediaRepository.InsertMediaServerConfigAsync(Data);


                result = new Response<string>()
                {
                    RespCode = ErrorCodes.Success,
                    Data = "MediaServerConfig has been Created."
                };

            }
            catch (Exception ex)
            {
                result = new Response<LogExceptionModel>()
                {
                    RespCode = ErrorCodes.InternalServerError,
                    Data = new LogExceptionModel(ex)
                };
            }
            return result;

        }

        public async Task<Response> UploadAsync(IFormFile file, string dirPath, string fileName, string bucket)
        {
            Response result = null;
            try
            {
                _mediaConfig = await _mediaRepository.GetMediaServerConfigAsync(_headersUtil.GetAccessKey());
                if (_mediaConfig == null)
                {
                    return result = new Response<string>()
                    {
                        RespCode = ErrorCodes.ForbiddenBlockMediaServerKey,
                        Data = "Forbidden Block MediaServerKey"
                    };
                } 

                FileResponse data = null; 
                IFileHelper _fileHelper = null;
                var StorageType = await _mediaRepository.GetMSTStorageTypeAsync(_mediaConfig.StorageTypeID);
                if (StorageTypeKey.S3 == StorageType.Key)
                {
                    _fileHelper = new FileHelperS3(_mediaConfig);
                }
                else if (StorageTypeKey.MinIO == StorageType.Key)
                {
                    _fileHelper = new FileHelperMinIO(_mediaConfig);
                }
                 
                var FileResp = await _fileHelper.UploadFile(file, dirPath, fileName, bucket);
                data = new FileResponse {
                    BucketName = FileResp.BucketName,
                    Name = FileResp.Name,
                    Url = FileResp.Url,
                    IsTemp = false
                };

                result = new Response<FileResponse>()
                {
                    RespCode = ErrorCodes.Success,
                    Data = data
                };

            }
            catch (Exception ex)
            {
                result = new Response<LogExceptionModel>()
                {
                    RespCode = ErrorCodes.InternalServerError,
                    Data = new LogExceptionModel(ex)
                };
            }
            return result;

        }

        public async Task<Response> UploadToTempAsync(IFormFile file)
        {
            Response result = null;
            try
            {
                _mediaConfig = await _mediaRepository.GetMediaServerConfigAsync(_headersUtil.GetAccessKey());

                //Check Header AccessKey StorageServer 
                if (_mediaConfig == null)
                {
                    return result = new Response<string>()
                    {
                        RespCode = ErrorCodes.ForbiddenBlockMediaServerKey,
                        Data = "Forbidden Block MediaServerKey"
                    };
                }
                var StorageType = await _mediaRepository.GetMSTStorageTypeAsync(_mediaConfig.StorageTypeID);

                FileResponse data = null;

                IFileHelper _fileHelper = null;
                
                if (StorageTypeKey.S3 == StorageType.Key)
                {
                    _fileHelper = new FileHelperS3(_mediaConfig);
                }
                else if(StorageTypeKey.MinIO == StorageType.Key)
                {
                    _fileHelper = new FileHelperMinIO(_mediaConfig);
                }

                string FileName = Guid.NewGuid().ToString() + "_" + file.Name;

                var FileResp = await _fileHelper.UploadFile(file, null, FileName, _mediaConfig.TempBucket);
                data = new FileResponse { 
                    BucketName = FileResp.BucketName,
                    Name = FileResp.Name,
                    Url = FileResp.Url,
                    IsTemp = true
                };

                result = new Response<FileResponse>()
                {
                    RespCode = ErrorCodes.Success,
                    Data = data
                };

            }
            catch (Exception ex)
            {
                result = new Response<LogExceptionModel>()
                {
                    RespCode = ErrorCodes.InternalServerError,
                    Data = new LogExceptionModel(ex)
                };
            }
            return result;

        }

        public async Task<Response> GetBucketListAsync()
        {
            Response result = null;
            try
            {
                _mediaConfig = await _mediaRepository.GetMediaServerConfigAsync(_headersUtil.GetAccessKey());

                //Check Header AccessKey StorageServer 
                if (_mediaConfig == null)
                {
                    return result = new Response<string>()
                    {
                        RespCode = ErrorCodes.ForbiddenBlockMediaServerKey,
                        Data = "Forbidden Block MediaServerKey"
                    };
                }
                var StorageType = await _mediaRepository.GetMSTStorageTypeAsync(_mediaConfig.StorageTypeID);
                 

                IFileHelper _fileHelper = null;

                if (StorageTypeKey.S3 == StorageType.Key)
                {
                    _fileHelper = new FileHelperS3(_mediaConfig);
                }
                else if (StorageTypeKey.MinIO == StorageType.Key)
                {
                    _fileHelper = new FileHelperMinIO(_mediaConfig);
                }

               var data =  await _fileHelper.GetBucketListAsync();

                var resp = data.Select(o => new BucketResponse { Name = o.Name }).ToList();

               result = new Response<List<BucketResponse>>()
                {
                    RespCode = ErrorCodes.Success,
                    Data = resp
               };

            }
            catch (Exception ex)
            {
                result = new Response<LogExceptionModel>()
                {
                    RespCode = ErrorCodes.InternalServerError,
                    Data = new LogExceptionModel(ex)
                };
            }
            return result;

        }
        public async Task<Response> GetFileAsyncAsync(string bucket, string dirPath, string fileName)
        {
            Response result = null;
            try
            {
                _mediaConfig = await _mediaRepository.GetMediaServerConfigAsync(_headersUtil.GetAccessKey());

                //Check Header AccessKey StorageServer 
                if (_mediaConfig == null)
                {
                    return result = new Response<string>()
                    {
                        RespCode = ErrorCodes.ForbiddenBlockMediaServerKey,
                        Data = "Forbidden Block MediaServerKey"
                    };
                }
                var StorageType = await _mediaRepository.GetMSTStorageTypeAsync(_mediaConfig.StorageTypeID);


                IFileHelper _fileHelper = null;

                if (StorageTypeKey.S3 == StorageType.Key)
                {
                    _fileHelper = new FileHelperS3(_mediaConfig);
                }
                else if (StorageTypeKey.MinIO == StorageType.Key)
                {
                    _fileHelper = new FileHelperMinIO(_mediaConfig);
                }

                var data = await _fileHelper.GetFileUrlAsync(bucket, dirPath, fileName);
                var resp = new FileResponse { 
                    BucketName = bucket,
                    Name = fileName,
                    Url = data,
                    IsTemp = bucket == _mediaConfig.TempBucket ? true : false, 
                };

                result = new Response<FileResponse>()
                {
                    RespCode = ErrorCodes.Success,
                    Data = resp
                };

            }
            catch (Exception ex)
            {
                result = new Response<LogExceptionModel>()
                {
                    RespCode = ErrorCodes.InternalServerError,
                    Data = new LogExceptionModel(ex)
                };
            }
            return result;
        }


    }
}
