using AP_MediaService.Common.Helper.Logging;
using AP_MediaService.Common.Helper;
using AP_MediaService.Common.Interfaces;
using AP_MediaService.Common.MappingErrors;
using AP_MediaService.Common.Models.ResponseModels;
using AP_MediaService.Common.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.ActionFilters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        private readonly IHeadersUtils _headersUtil;
        private readonly ILogger _logger;
        private readonly ILogService _logService;
        public ValidateModelAttribute(ILogger<ValidateModelAttribute> logger, IHeadersUtils headersUtil, ILogService logService)
        {
            _logger = logger;
            _headersUtil = headersUtil;
            _logService = logService;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                switch (context.RouteData.Values["Action"])
                {
                    case "Post":
                        ControllerHeader.setCommandName("Create");
                        break;
                    case "Put":
                        ControllerHeader.setCommandName("Update");
                        break;
                    case "Delete":
                        ControllerHeader.setCommandName("Delete");
                        break;
                    default:
                        break;
                }
                LogModel logModel = new LogModel(ControllerHeader.CommandName, null);

                Response result = null;
                //context.ActionArguments.FirstOrDefault() 
                Dictionary<string, string> errorList = context.ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                    );

                result = new Response<Dictionary<string, string>>()
                {
                    RespCode = ErrorCodes.Forbidden,
                    Data = errorList
                };
                //context.Result = HttpResultHelper.CustomResult(context.HttpContext.Request, ErrorCodes.Forbidden.GetHttpStatusCode(),
                //    new Response<Dictionary<string, string>>()
                //    {
                //        RespCode = ErrorCodes.Forbidden,
                //        Data = errorList
                //    });
                context.Result = HttpResultHelper.CustomResult(result.RespCode.GetHttpStatusCode(), result);
                logModel.ResponseData = result;
                _ = _logService.WriteLog(logModel);


            }

            return base.OnActionExecutionAsync(context, next);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {

            if (!context.ModelState.IsValid)
            {
                context.Result = HttpResultHelper.CustomResult(ErrorCodes.BadRequestInvalid.GetHttpStatusCode(), context.ModelState);
            }
        }
    }
}
