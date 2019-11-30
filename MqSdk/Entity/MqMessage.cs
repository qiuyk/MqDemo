using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqSdk.Entity
{
    public class MqMessage
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public string MessageID { get; set; }
        /// <summary>
        /// 发送人ID
        /// </summary>
        public string SenderID { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string ReceiverID { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string MessageTitle { get; set; }
        /// <summary>
        /// 消息体
        /// </summary>
        public string MessageBody { get; set; }

    }
}
