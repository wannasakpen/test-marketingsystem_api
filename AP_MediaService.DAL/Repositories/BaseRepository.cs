using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.DAL.Repositories
{
    public abstract class BaseRepository
    {
        private readonly string _ConnectionString;
        private readonly IConfiguration _config;
        protected int ConnctionLongQuerryTimeOut = 600;

        public BaseRepository(IConfiguration config)
        {
            _config = config;
            _ConnectionString = _config.GetConnectionString("DefaultConnection");
        }

        protected IDbConnection WebConnection
        {
            get
            {
                var conn = Environment.GetEnvironmentVariable("DefaultConnection");
                return new SqlConnection(conn);
            }
        }

       
    }
}
