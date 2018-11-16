using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.Message
{
    /// <summary>
    /// 微信消息推送队列
    /// </summary>
    [Table("QueueWeChatMsg")]
    public class QueueWeChatMsg : BaseEntity
    {
        public int ToUserId { get; set; }
        public int MessageId { get; set; }
        public string Body { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime? SentOnUtc { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// 发送次数
        /// </summary>
        public int SentTries { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public bool SendResult { get; set; }

        /// <summary>
        /// 发送结果
        /// </summary>
        public string ResultMsg { get; set; }

    }
}
