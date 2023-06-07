using AP_MediaService.Common.Helper.Logging;
using AP_MediaService.Common.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Helper.Interface
{
    public interface IHttpResultHelper
    {
        Task<ActionResult> CustomResult(HttpStatusCode statuscode, Response data, LogModel logModel);
    }
}
