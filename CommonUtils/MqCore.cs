using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace MqSdk
{
    internal class MqCore
    {
        internal void Publish(string exchange,string type,string queue,MqMessage message,string receiver)
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
                    channel.QueueBind(queue, exchange, receiver, null);
                    //发布消息
                    channel.BasicPublish(exchange, receiver, null, Encoding.UTF8.GetBytes(sendMessage));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void Subscribe(string exchange, string type, string queue, string receiver)
        {
            try
            {
                //信道
                using (var channel = MqConnection.GetMqConnection().CreateModel())
                {
                    //Exchange
                    channel.ExchangeDeclare(exchange, type, true, false, null);
                    //队列声明
                    channel.QueueDeclare(queue, true, false, false, null);
                    //队列绑定
                    channel.QueueBind(queue, exchange, receiver, null);
                    //消费者
                    var consumer = new EventingBasicConsumer(channel);
                    //触发事件
                    consumer.Received += Consumer_Received;
                    //消息应答
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
        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var msg = Encoding.UTF8.GetString(e.Body);
            MqMessage message = JsonConvert.DeserializeObject<MqMessage>(msg);
            MessageListening("mq", message);
        }
    }
}
