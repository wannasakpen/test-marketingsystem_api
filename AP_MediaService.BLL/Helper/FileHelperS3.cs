using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AP_MediaService.BLL.Helper.Interfaces;
using AP_MediaService.BLL.Models;
using AP_MediaService.Common.Extensions;
using AP_MediaService.Common.Models;
using AP_MediaService.DAL.Models.GenerateModels;
using AP_MediaService.DTO.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.BLL.Helper
{
    internal class FileHelperS3 : IFileHelper
    {
        private int _expireHours = 24; 
        private MSTMediaServerConfig _mediaConfig;
        public FileHelperS3(MSTMediaServerConfig mediaConfig)
        {
            _mediaConfig = mediaConfig; 
        }

        public async Task<FileUploadResult> UploadFile(IFormFile file, string? dirPath, string fileName, string bucket)
        {
            try
            {
                AmazonS3Client s3Client = new AmazonS3Client(
                        _mediaConfig.AccessKey,
                        _mediaConfig.SecretKey,
                        RegionEndpoint.APSoutheast1
                        );

                var stream = file.OpenReadStream();
                string objectName = $"{Guid.NewGuid()}_{file.FileName}";

                // resize image
                var extension = Path.GetExtension(file.FileName);

                var encoder = Extension.GetFileEncoder(extension);
                if (stream.Length >= 1048576)
                {
                    if (encoder != null)
                    {
                        var output = new System.IO.MemoryStream();
                        using (Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(stream))
                        { 
                            image.Save(output, encoder);
                            output.Position = 0;
                            stream = output;
                            var xxx = output.Length;
                        }
                    }
                }

                PutObjectRequest request = new PutObjectRequest();
                request.BucketName = _mediaConfig.Endpoint;
                request.Key = $"{bucket}/{objectName}";
                request.ContentType = file.ContentType;
                request.CannedACL = S3CannedACL.PublicReadWrite;
                request.InputStream = stream;
                request.StorageClass = S3StorageClass.StandardInfrequentAccess;

                //isTemp
                if (bucket == _mediaConfig.TempBucket) request.TagSet.Add(new Tag { Key = "istemp", Value = "1" });

                PutObjectResponse response = await s3Client.PutObjectAsync(request);

                GetPreSignedUrlRequest getrequest = new GetPreSignedUrlRequest();
                getrequest.BucketName = _mediaConfig.Endpoint;
                getrequest.Key = $"{bucket}/{objectName}";
                getrequest.Expires = DateTime.Now.AddHours(_expireHours);
                getrequest.Protocol = Protocol.HTTPS;
                string url = s3Client.GetPreSignedURL(getrequest);

                var data = new FileUploadResult()
                {
                    BucketName = bucket,
                    Name = $"{objectName}",
                    Url = url, 
                };

                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<List<BucketModel>> GetBucketListAsync()
        {
            return null;
        }
        public async Task<string> GetFileUrlAsync(string bucket, string name)
        {
            AmazonS3Client s3Client = new AmazonS3Client(
                        _mediaConfig.AccessKey,
                        _mediaConfig.SecretKey,
                        RegionEndpoint.APSoutheast1
                        );

            GetPreSignedUrlRequest getrequest = new GetPreSignedUrlRequest();
            getrequest.BucketName = _mediaConfig.Endpoint; ;
            getrequest.Key = $"{_mediaConfig.DefaultBucket}/{name}";
            getrequest.Expires = DateTime.Now.AddHours(_expireHours);
            getrequest.Protocol = Protocol.HTTPS;
            string url = s3Client.GetPreSignedURL(getrequest);


            return url;
        }
        public async Task<string> GetFileUrlAsync(string bucket, string dirPath, string fileName)
        {
            AmazonS3Client s3Client = new AmazonS3Client(
                        _mediaConfig.AccessKey,
                        _mediaConfig.SecretKey,
                        RegionEndpoint.APSoutheast1
                        );

            GetPreSignedUrlRequest getrequest = new GetPreSignedUrlRequest();
            getrequest.BucketName = _mediaConfig.Endpoint; ;
            getrequest.Key = $"{_mediaConfig.DefaultBucket}/{fileName}";
            getrequest.Expires = DateTime.Now.AddHours(_expireHours);
            getrequest.Protocol = Protocol.HTTPS;
            string url = s3Client.GetPreSignedURL(getrequest);


            return url;
        }
    }
}
