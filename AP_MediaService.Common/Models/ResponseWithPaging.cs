using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Models
{
    public class ResponseWithPaging<T>
    {
        public PageOutput Paging { get; set; }
        public T Value { get; set; }

        public ResponseWithPaging()
        {
            Paging = new PageOutput();
        }
    }

    public class ResponseWithPaging<H, D>
    {
        public PageOutput Paging { get; set; }
        public H Header { get; set; }
        public D Detail { get; set; }

        public ResponseWithPaging()
        {
            Paging = new PageOutput();
        }
    }

    public class ResponseHeaderWithDetail<H, D>
    {
        public H Header { get; set; }
        public D Detail { get; set; }
    }
}
