using System;

namespace MqSdk
{
    public class MqBuilder
    {

        private string exchange = "defaultexchange";
        private MqEnum type = MqEnum.Direct;
        private string queue = "defaultqueue";
        private string receiver;
        private MqMessage message;
        private EventHandler<MqMessage> listening;

        public MqBuilder withExchange(string exchange)
        {
            this.exchange = exchange;
            return this;
        }

        public MqBuilder withType(MqEnum type)
        {
            this.type = type;
            return this;
        }

        public MqBuilder withQueue(string queue)
        {
            this.queue = queue;
            return this;
        }
        public MqBuilder withReceiver(string receiver)
        {
            this.receiver = receiver;
            return this;
        }
        public MqBuilder withMessage(MqMessage message)
        {
            this.message = message;
            return this;
        }

        public MqBuilder withListening(EventHandler<MqMessage> listening)
        {
            this.listening = listening;
            return this;
        }

        public static MqBuilder GetInstance()
        {
            return new MqBuilder();
        }

        /// <summary>
        /// 消息发送
        /// 【必填】withMessage指定消息
        /// 【必填】withReceiver指定接收者以便发送到属于接受者的消息，格式请查MqEnum中注释（依赖于withType传入类型）
        /// 【非必填】withType指定消息类型 默认1对1
        /// </summary>
        public void SendMessage()
        {
            if (message == null)
            {
                throw new Exception("MQ未传入消息");
            }
            if (string.IsNullOrEmpty(receiver))
            {
                throw new Exception("MQ未传入接受者");
            }
            MqCore mqCore = new MqCore();
            mqCore.Publish(exchange, type.ToString(), queue, message,receiver);
        }

        /// <summary>
        /// 消息监听
        /// 【必填】withListening指定接收事件
        /// 【必填】withReceiver指定接收者以便收到属于这个接受者的消息，格式请查MqEnum中注释（依赖于withType传入类型）
        /// 【非必填】withType指定消息类型 默认1对1
        /// </summary>
        public void Listening()
        {
            if (listening == null)
            {
                throw new Exception("MQ未注册监听事件");
            }
            if (string.IsNullOrEmpty(receiver))
            {
                throw new Exception("MQ未传入接受者");
            }
            MqCore mqCore = new MqCore();
            mqCore.MessageListening += listening;
            mqCore.Subscribe(exchange, type.ToString(), queue, receiver);
        }
    }
}
