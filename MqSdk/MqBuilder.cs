using MqSdk.Core;
using MqSdk.Entity;
using MqSdk.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MqSdk
{
    public class MqBuilder
    {
        #region 私有成员

        private MqEnum type = MqEnum.Topic;
        private string receiver;
        private string role;
        private MqMessage message;
        private EventHandler<MqMessage> listening;
        private EventHandler<Exception> asyncException;
        private bool isAsync = false;

        #endregion

        #region 构建方法

        public MqBuilder withType(MqEnum type = MqEnum.Topic)
        {
            this.type = type;
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

        public MqBuilder withRole(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentException("传入参数错误");
            }
            this.role = role;
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

        public MqBuilder withAsyncException(EventHandler<Exception> asyncException)
        {
            if (asyncException == null)
            {
                throw new ArgumentException("传入参数错误");
            }
            this.asyncException = asyncException;
            return this;
        }

        #endregion

        #region 创建构建器

        public static MqBuilder CreateBuilder()
        {
            return new MqBuilder();
        }

        #endregion

        #region 消息发送

        /// <summary>
        /// 消息发送
        /// 
        /// 【必填】withMessage指定消息
        /// 
        /// 【必填】withReceiver指定接收者以便发送到属于接受者的消息，格式请查MqEnum中注释（依赖于withType传入类型）
        /// 
        /// 【非必填】withType指定消息类型 默认1对1
        /// </summary>
        public void SendMessage()
        {
            ValidateSendParams();

            string routingKey = GetPublishRoutingKey();

            string queue = "publish_" + Config.MqConfig.AppID + "_" + type.ToString().ToLower();

            MqCore.GetInstance().Publish(type.ToString().ToLower(), queue, message, routingKey);
        }


        /// <summary>
        ///  异步发送消息
        ///  
        /// 【必填】withListening指定接收事件
        /// 
        /// 【必填】withReceiver指定接收者以便收到属于这个接受者的消息，格式请查MqEnum中注释（依赖于withType传入类型）
        /// 
        /// 【非必填】withType指定消息类型
        /// 
        /// 【非必填】使用异步建议通过withAsyncException传入异常事件接收
        /// </summary>
        public void SendMessageAsync()
        {
            isAsync = true;
            var task = Task.Factory.StartNew(SendMessage);
            task.ContinueWith(s => asyncException("mq", s.Exception));
        }

        #endregion

        #region 消息监听

        /// <summary>
        /// 消息监听
        /// 
        /// 【必填】withListening指定接收事件
        /// 
        /// 【必填】withReceiver指定接收者以便收到属于这个接受者的消息，格式请查MqEnum中注释（依赖于withType传入类型）
        /// 
        /// 【非必填】withType指定消息类型 默认1对1
        /// </summary>
        public void Listening()
        {
            ValidateLesteningParams();

            List<string> listRoutingKey = GetListeningRoutingKey(type);

            string queue = "listening_" + Config.MqConfig.AppID + "_" + type.ToString().ToLower() +"_" + receiver;

            MqCore mqCore = new MqCore();

            mqCore.MessageListening += listening;

            mqCore.Subscribe(type.ToString().ToLower(), queue, listRoutingKey);
        }

        /// <summary>
        /// 消息监听
        /// 
        /// 【必填】withListening指定接收事件
        /// 
        /// 【必填】withReceiver指定接收者以便收到属于这个接受者的消息，格式请查MqEnum中注释（依赖于withType传入类型）
        /// 
        /// 【非必填】withType指定消息类型 默认1对1
        /// 
        /// 【非必填】使用异步建议通过withAsyncException传入异常事件接收
        /// </summary>
        public void ListeningAsync()
        {
            isAsync = true;
            ValidateLesteningParams();
            var task = Task.Factory.StartNew(Listening);
            task.ContinueWith(s => asyncException("mq", s.Exception));
        }

        #endregion

        #region routingKey获取

        /// <summary>
        /// 获取监听RoutingKey
        /// </summary>
        /// <returns></returns>
        private List<string> GetListeningRoutingKey(MqEnum mqEnum)
        {
            List<string> listRoutingKey = new List<string>();

            switch (mqEnum)
            {
                case MqEnum.Fanout:
                    listRoutingKey.Add("all");
                    break;
                case MqEnum.Topic:
                    listRoutingKey.Add("*." + receiver);
                    listRoutingKey.Add(role + ".*");
                    break;
                case MqEnum.Direct:
                    listRoutingKey.Add(receiver);
                    break;
                default:
                    break;
            }
            
            return listRoutingKey;
        }

        /// <summary>
        /// 获取发布RoutingKey
        /// </summary>
        /// <param name="mqEnum"></param>
        /// <returns></returns>
        private string GetPublishRoutingKey()
        {
            string routingKey = string.Empty;
            switch (type)
            {
                case MqEnum.Fanout:
                    //fanout会忽略routingkey路由到所有与交换机绑定的队列,因需要绑定队列因此给默认值
                    routingKey = "all";
                    break;
                case MqEnum.Topic:
                    if (string.IsNullOrEmpty(role))
                    {
                        //如果未传入role代表1对1
                        routingKey = receiver + "." + receiver;
                    }
                    else
                    {
                        //如果传入角色代表1对多
                        routingKey = role + "." + role;
                    }
                    break;
                case MqEnum.Direct:
                    routingKey = receiver;
                    break;
                default:
                    break;
            }

            return routingKey;
        }

        #endregion

        #region 参数校验

        /// <summary>
        /// 校验发送消息参数
        /// </summary>
        private void ValidateSendParams()
        {
            
            if (message == null)
            {
                throw new Exception("MQ未传入消息");
            }
            if (isAsync && asyncException == null)
            {
                throw new Exception("MQ未注册消息监听异常事件");
            }
            if (type.Equals(MqEnum.Topic))
            {
                if (string.IsNullOrEmpty(receiver) && string.IsNullOrEmpty(role))
                {
                    throw new Exception("MQ未传入接收者ID或角色");
                }
            }
            if (type.Equals(MqEnum.Direct))
            {
                if (string.IsNullOrEmpty(receiver))
                {
                    throw new Exception("MQ未传入接收者ID");
                }
            }
        }

        /// <summary>
        /// 校验监听参数
        /// </summary>
        private void ValidateLesteningParams()
        {
            if (receiver == null)
            {
                throw new Exception("MQ未传入接收者ID");
            }
            if (listening == null)
            {
                throw new Exception("MQ未注册消息监听消息事件");
            }
            if (isAsync && asyncException == null)
            {
                throw new Exception("MQ未注册消息监听异常事件");
            }
            if (type.Equals(MqEnum.Topic))
            {
                if (string.IsNullOrEmpty(role))
                {
                    throw new Exception("MQ未传入接收者角色");
                }
            }
        }

        #endregion
    }
}
