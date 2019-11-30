using System;
using System.Threading.Tasks;

namespace MqSdk
{
    public class MqBuilder
    {

        #region 私有成员

        private string exchange = "defaultexchange";
        private MqEnum type = MqEnum.Topic;
        private string queue = "defaultqueue";
        private string receiver;
        private MqMessage message;
        private EventHandler<MqMessage> listening;
        private EventHandler<Exception> asyncexception;

        #endregion

        #region 构建方法
        public MqBuilder withExchange(string exchange)
        {
            if (string.IsNullOrEmpty(exchange))
            {
                throw new ArgumentException("传入参数错误");
            }
            this.exchange = exchange;
            return this;
        }

        public MqBuilder withType(MqEnum type = MqEnum.Topic)
        {
            this.type = type;
            return this;
        }

        public MqBuilder withQueue(string queue)
        {
            if (string.IsNullOrEmpty(queue))
            {
                throw new ArgumentException("传入参数错误");
            }
            this.queue = queue;
            return this;
        }
        public MqBuilder withReceiver(string receiver)
        {
            if (string.IsNullOrEmpty(receiver))
            {
                throw new ArgumentException("传入参数错误");
            }
            this.receiver = receiver;
            return this;
        }
        public MqBuilder withMessage(MqMessage message)
        {
            if (message == null)
            {
                throw new ArgumentException("传入参数错误");
            }
            this.message = message;
            return this;
        }

        public MqBuilder withListening(EventHandler<MqMessage> listening)
        {
            if (listening == null)
            {
                throw new ArgumentException("传入参数错误");
            }
            this.listening = listening;
            return this;
        }

        public MqBuilder withAsyncException(EventHandler<Exception> asyncexception)
        {
            if (asyncexception == null)
            {
                throw new ArgumentException("传入参数错误");
            }
            this.asyncexception = asyncexception;
            return this;
        }
        

        public static MqBuilder GetInstance()
        {
            return new MqBuilder();
        }

        #endregion

        #region 公共方法

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
            mqCore.Publish(exchange, type.ToString().ToLower(), queue, message, receiver);
        }

        /// <summary>
        ///  异步发送消息
        /// 【必填】withListening指定接收事件
        /// 【必填】withReceiver指定接收者以便收到属于这个接受者的消息，格式请查MqEnum中注释（依赖于withType传入类型）
        /// 【非必填】withType指定消息类型 默认1对1
        /// 【非必填】使用异步建议通过withAsyncException传入异常事件接收
        /// </summary>
        public void SendMessageAsync()
        {
            var task = Task.Factory.StartNew(SendMessage);
            task.ContinueWith(s => asyncexception("mq", s.Exception));
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
            mqCore.Subscribe(exchange, type.ToString().ToLower(), queue, receiver);
        }

        /// <summary>
        /// 消息监听
        /// 【必填】withListening指定接收事件
        /// 【必填】withReceiver指定接收者以便收到属于这个接受者的消息，格式请查MqEnum中注释（依赖于withType传入类型）
        /// 【非必填】withType指定消息类型 默认1对1
        /// 【非必填】使用异步建议通过withAsyncException传入异常事件接收
        /// </summary>
        public void ListeningAsync()
        {
            var task = Task.Factory.StartNew(Listening);
            task.ContinueWith(s => asyncexception("mq", s.Exception));
        }

        #endregion

    }
}
