

using AP_MediaService.Common.MappingErrors;

namespace AP_MediaService.Common.Models.ResponseModels
{
    public class Response<T> : Response
    {
        public T Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>The requested operation was successfully.</example>
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
    }

    public abstract class Response
    {
        public ErrorCodes RespCode { get; set; }
    }
}
