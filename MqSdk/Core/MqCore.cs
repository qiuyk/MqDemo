using MqSdk.Entity;
using MqSdk.Utils;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MqSdk.Core
{
    /// <summary>
    /// MQ操作类
    /// </summary>
    internal class MqCore
    {

        #region MQ交互

        internal static MqCore GetInstance()
        {
            return new MqCore();
        }

        /// <summary>
        /// 消息发布
        /// </summary>
        /// <param name="exchange">交换器</param>
        /// <param name="type">交换器类型</param>
        /// <param name="queue">消息队列</param>
        /// <param name="message">消息</param>
        /// <param name="routingKey">路由键</param>
        internal void Publish(string type,string queue,MqMessage message,string routingKey)
        {
            try
            {
                string exchange = Config.MqConfig.AppID + "_" + type.ToString().ToLower();

                string sendMessage = JsonConvert.SerializeObject(message);
                //信道
                using (var channel = MqConnection.GetMqConnection().CreateModel())
                {
                    //Exchange
                    channel.ExchangeDeclare(exchange, type, true, false, null);
                    //声明队列
                    channel.QueueDeclare(queue, true, false, false, null);
                    //队列绑定
                    channel.QueueBind(queue, exchange, routingKey, null);
                    IBasicProperties props = channel.CreateBasicProperties();
                    props.ContentType = "json";
                    props.DeliveryMode = 2;
                    //过期时间
                    //props.Expiration = "36000000";
                    //发布消息
                    channel.BasicPublish(exchange, routingKey, props, Encoding.UTF8.GetBytes(sendMessage));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 消息订阅
        /// </summary>
        /// <param name="exchange">交换器</param>
        /// <param name="type">交换器类型</param>
        /// <param name="queue">消息队列</param>
        /// <param name="routingKey">路由键</param>
        internal void Subscribe(string type, string queue, List<string> listRoutingKey)
        {
            try
            {
                string exchange = Config.MqConfig.AppID + "_" + type.ToString().ToLower();
                //信道
                var channel = MqConnection.GetMqConnection().CreateModel();
                //Exchange
                channel.ExchangeDeclare(exchange, type, true, false, null);
                //声明队列
                channel.QueueDeclare(queue, true, false, false, null);
                //队列绑定
                listRoutingKey.ForEach(routingKey => 
                {
                    channel.QueueBind(queue, exchange, routingKey, null);
                });
                
                //消费者
                var consumer = new EventingBasicConsumer(channel);
                
                //触发事件
                consumer.Received += (sender, ea)=> 
                {
                    var msg = Encoding.UTF8.GetString(ea.Body);

                    MqMessage message = JsonConvert.DeserializeObject<MqMessage>(msg);

                    MessageListening(sender, message);

                    //确认接收
                    channel.BasicAck(ea.DeliveryTag, false);
                };

                //程序主动应答
                channel.BasicConsume(queue: queue,
                    autoAck: false,
                    consumer: consumer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 消息监听
        /// </summary>
        internal event EventHandler<MqMessage> MessageListening;

        #endregion

    }
}
