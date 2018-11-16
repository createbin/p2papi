using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.WeChat
{
    [Table("WeChat_Push_Template")]
    public class WeChatPushTemplate : BaseEntity
    {
        public string template_id { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public string url { get; set; }

        public string appid { get; set; }

        public string pagepath { get; set; }

        public string remark { get; set; }

        public bool default_enabled { get; set; }

        public bool enabled { get; set; }
    }
}