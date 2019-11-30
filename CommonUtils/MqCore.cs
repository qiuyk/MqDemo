using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace MqSdk
{
    /// <summary>
    /// MQ核心类
    /// </summary>
    internal class MqCore
    {

        #region MQ操作

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
        internal void Publish(string exchange,string type,string queue,MqMessage message,string routingKey)
        {
            try
            {
                string sendMessage = JsonConvert.SerializeObject(message);
                //信道
                using (var channel = MqConnection.GetMqConnection().CreateModel())
                {
                    //Exchange
                    channel.ExchangeDeclare(exchange, type, true, false, null);
                    ////声明队列
                    channel.QueueDeclare(queue, true, false, false, null);
                    ////队列绑定
                    channel.QueueBind(queue, exchange, routingKey, null);
                    //发布消息
                    channel.BasicPublish(exchange, routingKey, null, Encoding.UTF8.GetBytes(sendMessage));
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
        internal void Subscribe(string exchange, string type, string queue, string routingKey)
        {
            try
            {
                //信道
                using (var channel = MqConnection.GetMqConnection().CreateModel())
                {
                    //Exchange
                    channel.ExchangeDeclare(exchange, type, true, false, null);

                    //消费者
                    var consumer = new EventingBasicConsumer(channel);
                    
                    //触发事件
                    consumer.Received += (sender, ea)=> 
                    {
                        var msg = Encoding.UTF8.GetString(ea.Body);
                        MqMessage message = JsonConvert.DeserializeObject<MqMessage>(msg);
                        

                        MessageListening("mq", message);

                        //using (var channelAsk = MqConnection.GetMqConnection().CreateModel())
                        //{
                        //    //确认接收
                        //    channelAsk.BasicAck(ea.DeliveryTag, false);
                        //}
                    };

                    //程序主动应答
                    channel.BasicConsume(queue: queue,
                        autoAck: true,
                        consumer: consumer);
                }
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

        /// <summary>
        /// 转化监听
        /// </summary>
        //private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        //{
            
        //}

        #endregion

    }
}
