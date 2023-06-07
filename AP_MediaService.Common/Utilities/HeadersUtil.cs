using AP_MediaService.Common.Extensions;
using AP_MediaService.Common.Interfaces;
using AP_MediaService.Common.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Utilities
{
    public class HeadersUtil : IHeadersUtils
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string UserID { get; set; }
        public string RoleCode { get; set; }
        public string ControllerName { get; set; }


        public HeadersUtil(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public Guid? GetUserID()
        {
            Guid? CurrentUserID;

            Guid parsedUserID;
            if (Guid.TryParse(_httpContextAccessor?.HttpContext?.User?.Claims.Where(x => x.Type == "userid").Select(o => o.Value).SingleOrDefault(), out parsedUserID))
            {
                CurrentUserID = parsedUserID;
            }
            else
                CurrentUserID = null;

            return CurrentUserID;
        }
        public string GetRoleCode()
        {
            string RoleCode;
            RoleCode = _httpContextAccessor?.HttpContext?.User?.Claims.Where(x => x.Type == ClaimTypes.Role).Select(o => o.Value).SingleOrDefault();
            this.RoleCode = RoleCode;
            return this.RoleCode;
        }
        public string GetMethod()
        {
            return _httpContextAccessor.HttpContext.Request.Method.ToString();
            //return this._httpContextAccessor.HttpContext.Request.Headers["Method"].ToString(); ;
        }

        public string GetAccessKey()
        {
            return _httpContextAccessor.HttpContext.Request?.Headers["AccessKey"].ToString() ?? "AKIAVHXJH2VQ34HO25GR";
            //return this._httpContextAccessor.HttpContext.Request.Headers["Method"].ToString(); ;
        }

        public string SetMethod(string SetMethod)
        {
            _httpContextAccessor.HttpContext.Request.Headers.Add("Method", SetMethod);
            return null;
        }

        public string setPagingHeader(PageOutput output)
        {
            if (output != null)
            {
                _httpContextAccessor.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Paging-PageNo, X-Paging-PageSize, X-Paging-PageCount, X-Paging-TotalRecordCount");
                _httpContextAccessor.HttpContext.Response.Headers.Add("X-Paging-PageNo", output.Page.ToString());
                _httpContextAccessor.HttpContext.Response.Headers.Add("X-Paging-PageSize", output.PageSize.ToString());
                _httpContextAccessor.HttpContext.Response.Headers.Add("X-Paging-PageCount", output.PageCount.ToString());
                _httpContextAccessor.HttpContext.Response.Headers.Add("X-Paging-TotalRecordCount", output.RecordCount.ToString());
            }
            return null;
        }

        public string GeturlPath()
        {
            string host = _httpContextAccessor.HttpContext.Request.Host.ToString();
            string path = _httpContextAccessor.HttpContext.Request.Path.ToString();
            string PathBase = _httpContextAccessor.HttpContext.Request.PathBase.ToString();
            //string QueryString = _httpContextAccessor.HttpContext.Request.QueryString.ToString();
            string fullPath = host + PathBase + path;

            return fullPath;
        }

        public string GetQueryString()
        {
            return _httpContextAccessor.HttpContext.Request.QueryString.ToString(); ;
        }

        public async Task<string> GetBodyRequest()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            var bodyStream = new StreamReader(request.Body);
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            string paramRequest = await bodyStream.ReadToEndAsync();
            if (Extension.IsValidJson(paramRequest) && !string.IsNullOrWhiteSpace(paramRequest))
            {
                paramRequest = JValue.Parse(paramRequest).ToString(Formatting.Indented);
            }

            return paramRequest;
        }

        public HttpRequest GetHttpRequest()
        {
            return this._httpContextAccessor.HttpContext.Request;
        }



    }
}
