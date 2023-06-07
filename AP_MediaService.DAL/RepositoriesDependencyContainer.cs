using AP_MediaService.DAL.Repositories;
using AP_MediaService.DAL.Repositories.IRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.DAL
{
    public class RepositoriesDependencyContainer
    {
        public static void RegisterRepositories(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<IMediaRepository, MediaRepository>();
        }
    }
}
