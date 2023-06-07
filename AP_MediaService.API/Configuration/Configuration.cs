namespace AP_MediaService.API.Configuration
{
    public class Configuration
    {
        private static IConfigurationRoot _config
        {
            get
            {
                string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"); //VAL = DEV,SIT,UAT,PRE,PRO
                if (string.IsNullOrEmpty(env)) env = "Development";

                return new ConfigurationBuilder()
                               //.SetBasePath(string.Format("{0}/config_{1}", AppDomain.CurrentDomain.BaseDirectory, env))
                               .AddJsonFile("appsettings.json")
                               .Build();
            }
        }

        public static string ConnectionString
        {
            get { return Environment.GetEnvironmentVariable("DefaultConnection") ?? _config.GetConnectionString("DefaultConnection"); }
        }
    }
}
