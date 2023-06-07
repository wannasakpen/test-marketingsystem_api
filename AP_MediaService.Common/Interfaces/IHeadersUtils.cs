using AP_MediaService.Common.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Interfaces
{
    public interface IHeadersUtils
    {
        Task<string> GetBodyRequest();
        string GetQueryString();
        HttpRequest GetHttpRequest();
        Guid? GetUserID();
        string GeturlPath();
        string GetRoleCode();
        string GetMethod();
        string SetMethod(string Method);
        string GetAccessKey();
        string setPagingHeader(PageOutput output);
        string UserID { get; set; }
        string RoleCode { get; set; }
        string ControllerName { get; set; }
    }
}
