using Microsoft.AspNetCore.Mvc;

namespace AP_MediaService.API.Controllers
{
    public class HealthCheckController : Controller
    {
        [Route("[controller]")]
        [HttpGet]
        public ContentResult HealthCheck()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                Content = "<div>HealthCheck a</div>"
            };
        }
    }
}
