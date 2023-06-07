using AP_MediaService.DAL.Models.GenerateModels; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.DAL.Repositories.IRepositories
{ 
    public interface IMediaRepository
    {
        Task<MSTMediaServerConfig> GetMediaServerConfigAsync(string MediaServerKey);
        Task<bool> InsertMediaServerConfigAsync(MSTMediaServerConfig Data);
        Task<MSTStorageType> GetMSTStorageTypeAsync(Guid StorageTypeID);
    }
}
