using System.Linq;
using ZFCTAPI.Core.Configuration;

namespace ZFCTAPI.Services.RabbitMQ
{
    /// MQ订阅管理
    /// <summary>
    /// </summary>
    public interface IRabbitMQSubscriptionsManager
    {
        /// <summary>
        /// 查询订阅
        /// </summary>
        RabbitProducer GetSubscription(string sendType);
    }

    public class RabbitMQSubscriptionsManager : IRabbitMQSubscriptionsManager
    {
        private readonly RabbitMQConfig _rabbitMQConfig;

        public RabbitMQSubscriptionsManager(RabbitMQConfig rabbitMQConfig)
        {
            _rabbitMQConfig = rabbitMQConfig;
        }

        public RabbitProducer GetSubscription(string sendType)
        {
            if (_rabbitMQConfig.Producers != null)
            {
                return _rabbitMQConfig.Producers.Where(p => p.SendType.Equals(sendType)).FirstOrDefault();
            }
            return null;
        }
    }
}