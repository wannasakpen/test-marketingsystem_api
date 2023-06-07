using Amazon.S3.Model;
using Amazon.S3;
using AP_MediaService.BLL.Helper.Interfaces;
using AP_MediaService.BLL.Models;
using AP_MediaService.Common.Models;
using AP_MediaService.DAL.Models.GenerateModels;
using AP_MediaService.DTO.Models.ResponseModels;
using Confluent.Kafka;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;

namespace AP_MediaService.BLL.Helper
{
    internal class FileHelperMinIO : IFileHelper
    { 
        private int _expireHours = 24;

     
        private MSTMediaServerConfig _mediaConfig;
        public FileHelperMinIO(MSTMediaServerConfig mediaConfig)
        {
            _mediaConfig = mediaConfig;
        }
         

        public async Task<FileUploadResult> UploadFile(IFormFile file, string? dirPath, string fileName, string bucket)
        {
            try
            {
                MinioClient minio = new MinioClient()
                                  .WithEndpoint(_mediaConfig.Endpoint)
                                  .WithCredentials(_mediaConfig.AccessKey, _mediaConfig.SecretKey)
                                  .WithSSL((bool)_mediaConfig.WithSSL)
                                  .Build();

                // Make a bucket on the server, if not already present.
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucket); 
                bool bucketExisted = await minio.BucketExistsAsync(beArgs); 

                if (!bucketExisted)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucket);
                    await minio.MakeBucketAsync(mbArgs);
                }

                var stream = file.OpenReadStream();
                string objectName = $"{dirPath}" + "/" + $"{fileName}";

                // Upload a file to bucket.
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(objectName)
                    .WithStreamData(stream)
                    .WithObjectSize(file.Length)
                    .WithContentType(file.ContentType);
                await minio.PutObjectAsync(putObjectArgs);

                var args = new PresignedGetObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(objectName)
                    .WithExpiry((int)TimeSpan.FromHours(_expireHours).TotalSeconds);
                var url = await minio.PresignedGetObjectAsync(args);

                  
                url = ReplaceWithPublicURL(url);
                return new FileUploadResult()
                {
                    Name = objectName,
                    BucketName = bucket,
                    Url = url
                };
            }
            catch (MinioException ex)
            {
                throw ex;
            }
        }

        public async Task<List<BucketModel>> GetBucketListAsync()
        {
            try
            {
                MinioClient minio = new MinioClient()
                                  .WithEndpoint(_mediaConfig.Endpoint)
                                  .WithCredentials(_mediaConfig.AccessKey, _mediaConfig.SecretKey)
                                  .WithSSL((bool)_mediaConfig.WithSSL)
                                  .Build();

                // List buckets that have read access.
                var list = await minio.ListBucketsAsync();

                List<BucketModel> Result = list.Buckets.Select(o => new BucketModel { Name = o.Name, CreationDate = o.CreationDate }).ToList();

                return Result;

            }
            catch (MinioException ex)
            {
                throw ex;
            }

        }

        //public async Task MoveAndRemoveFileAsync(string sourceBucket, string sourceObjectName, string destBucket, string destObjectName)
        //{ 

        //    MinioClient minio = new MinioClient()
        //                          .WithEndpoint(_mediaConfig.Endpoint)
        //                          .WithCredentials(_mediaConfig.AccessKey, _mediaConfig.SecretKey)
        //                          .WithSSL((bool)_mediaConfig.WithSSL)
        //                          .Build();


        //    string objectName = $"{dirPath}" + "/" + $"{desname}";

        //    await minio.CopyObjectAsync(tempbucket, tempFileName, desbucket, objectName);

        //    await minio.RemoveObjectAsync(tempbucket, tempFileName);
        //}

        public async Task<string> GetFileUrlAsync(string bucket, string dirPath, string fileName)
        { 
            try
            {
                MinioClient minio = new MinioClient()
                                 .WithEndpoint(_mediaConfig.Endpoint)
                                 .WithCredentials(_mediaConfig.AccessKey, _mediaConfig.SecretKey)
                                 .WithSSL((bool)_mediaConfig.WithSSL)
                                 .Build();

                string objectName = $"{dirPath}" + "/" + $"{fileName}";
                var args = new PresignedGetObjectArgs()
                .WithBucket(bucket)
                       .WithObject(objectName)
                       .WithExpiry((int)TimeSpan.FromHours(_expireHours).TotalSeconds);
                var url = await minio.PresignedGetObjectAsync(args);

                return ReplaceWithPublicURL(url);
            }
            catch (MinioException ex)
            { 
                throw ex;
            } 
        }

        private string ReplaceWithPublicURL(string url, string _minioEndpoint, string _publicURL)
        {
            if (!string.IsNullOrEmpty(_publicURL))
            {
                url = url.Replace("https://", "");
                url = url.Replace("http://", "");

                url = url.Replace(_minioEndpoint, _publicURL);
            }
            return url;
        }

        private string ReplaceWithPublicURL(string url)
        {
            if (!string.IsNullOrEmpty(_mediaConfig.PublicURL))  
            {
                url = url.Replace("https://", "");
                url = url.Replace("http://", "");

                url = url.Replace(_mediaConfig.Endpoint, _mediaConfig.PublicURL); 
            }
            return url;
        }

    }
}
