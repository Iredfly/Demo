using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto.WxAuth
{
    public class WxConfigOut
    {
        public string AppId { get; set; }
        public string Timestamp { get; set; }
        public string nonceStr { get; set; }
        public string Signature { get; set; }
        public string Url { get; set; }
        public string RawString { get; set; }

        public string Ticket { get; set; }
    }
}
