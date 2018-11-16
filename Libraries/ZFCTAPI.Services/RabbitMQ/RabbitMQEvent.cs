using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using ZFCTAPI.Data.RabbitMQ;

namespace ZFCTAPI.Services.RabbitMQ
{
    /// <summary>
    /// MQ事件
    /// </summary>
    public interface IRabbitMQEvent
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="model"></param>
        void Publish(BasePusblishModel model);
    }

    public class RabbitMQEvent : IRabbitMQEvent
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IRabbitMQSubscriptionsManager _subsManager;

        public RabbitMQEvent(IRabbitMQPersistentConnection persistentConnection, IRabbitMQSubscriptionsManager subsManager)
        {
            _persistentConnection = persistentConnection;
            _subsManager = subsManager;
        }

        public void Publish(BasePusblishModel model)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            var producer = _subsManager.GetSubscription(model.GetType().Name);

            if (producer == null)
                return;

            //报错重试
            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            using (var channel = _persistentConnection.CreateModel())
            {
                var message = JsonConvert.SerializeObject(model);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    channel.BasicPublish(exchange: producer.Exchange,
                                     routingKey: producer.RouteKey,
                                     basicProperties: null,
                                     body: body);
                });
            }
        }
    }
}