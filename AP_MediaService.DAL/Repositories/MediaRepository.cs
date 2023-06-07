using AP_MediaService.DAL.Models.GenerateModels;
using AP_MediaService.DAL.Repositories.IRepositories; 
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.DAL.Repositories
{
 
    public class MediaRepository : BaseRepository, IMediaRepository
    {
        private readonly IConfiguration _config;
        string TAG = "MediaRepository";
        public MediaRepository(IConfiguration config) : base(config)
        {
            _config = config;

        }
        public async Task<MSTMediaServerConfig> GetMediaServerConfigAsync(string MediaServerKey)
        {
            using (IDbConnection conn = WebConnection)
            {
                try
                {
                    conn.Open();

                    var ParamList = new DynamicParameters();
                    string sQuery = "SELECT * FROM MSTMediaServerConfig WHERE MediaServerKey = @MediaServerKey";
                    ParamList.Add("@MediaServerKey", MediaServerKey);
                    var result = (await conn.QueryAsync<MSTMediaServerConfig>(sQuery, ParamList)).ToList();


                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception($"{TAG}.GetMediaServerConfigAsync() :: Error ", ex);
                }
            }
        }

        public async Task<bool> InsertMediaServerConfigAsync(MSTMediaServerConfig Data)
        {
            using (IDbConnection conn = WebConnection)
            {
                conn.Open();
                using (IDbTransaction tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    { 
                        var resultTimeline = await conn.InsertAsync(Data, tran);

                        tran.Commit();
                        return true;


                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception($"{TAG}.InsertMediaServerConfigAsync() :: Error ", ex);
                    }
                }
            }
        }

        public async Task<MSTStorageType> GetMSTStorageTypeAsync(Guid StorageTypeID)
        {
            using (IDbConnection conn = WebConnection)
            {
                try
                {
                    conn.Open();

                    var ParamList = new DynamicParameters();
                    string sQuery = "SELECT * FROM MSTStorageType WHERE StorageTypeID = @StorageTypeID";
                    ParamList.Add("@StorageTypeID", StorageTypeID);
                    var result = (await conn.QueryAsync<MSTStorageType>(sQuery, ParamList)).ToList();


                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception($"{TAG}.GetMediaServerConfigAsync() :: Error ", ex);
                }
            }
        }
    }
}
