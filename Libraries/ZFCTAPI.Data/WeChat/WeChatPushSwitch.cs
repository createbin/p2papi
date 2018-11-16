using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.WeChat
{
    [Table("WeChat_Push_Switch")]
    public class WeChatPushSwitch : BaseEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int cst_customer_Id { get; set; }

        /// <summary>
        /// 模板类型
        /// </summary>
        public string template_type { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        public bool enable { get; set; }
    }
}