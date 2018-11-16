using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.Message
{
    /// <summary>
    /// 短信发送记录表
    /// </summary>
    [Table("QueuedSMS")]
    public class QueuedSMS : BaseEntity
    {
        /// <summary>
        /// 短信类型（1：广告；2：验证码；3：催费；4：语音；5：客户维护；6：其他；）
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 短信接收姓名
        /// </summary>
        public string ToName { get; set; }

        /// <summary>
        /// 短信接收号码
        /// </summary>
        public string ToPhone { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the sent date and time
        /// </summary>
        public DateTime? SentOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the sent date and time
        /// </summary>
        public DateTime? CreatedOnUtc { get; set; }

        /// <summary>
        /// 短信发送状态 是否成功？
        /// </summary>

        public bool SendResult { get; set; }

        /// <summary>
        /// 发送重试次数 最多3次
        /// </summary>
        public int SentTries { get; set; }
    }
}