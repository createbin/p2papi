using System.Collections.Generic;

namespace ZFCTAPI.Core.Configuration
{
    public class RabbitMQConfig
    {
        public RabbitMQConnection Connection { get; set; }

        /// <summary>
        /// 生产者列表
        /// </summary>
        public IEnumerable<RabbitProducer> Producers { get; set; }
    }

    public class RabbitMQConnection
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
    }

    /// <summary>
    /// 生产者
    /// </summary>
    public class RabbitProducer
    {
        /// <summary>
        /// 交换机
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        public string RouteKey { get; set; }

        /// <summary>
        /// 发送模型
        /// </summary>
        public string SendType { get; set; }
    }
}