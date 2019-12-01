using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MqSdk;
using MqSdk.Entity;

namespace UnitTestProject1
{
    [TestClass]
    public class ProductTest
    {
        /// <summary>
        /// 一对多发送
        /// 
        /// 场景：对全体人员广播
        /// </summary>
        [TestMethod]
        public void TestFanoutSend()
        {
            for (int i = 0; i < 1000; i++)
            {
                try
                {
                    //对全体人员广播
                    MqBuilder.CreateBuilder()
                        .withType(MqEnum.Fanout)
                        .withMessage(new MqMessage
                        {
                            SenderID = "System",
                            MessageID = Guid.NewGuid().ToString("N"),
                            MessageBody = "系统将夜晚0点不停服更新",
                            MessageTitle = "系统提醒",
                        })
                        .SendMessage();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 一对多发送
        /// 
        /// 场景：审批通知，即时聊天
        /// </summary>
        [TestMethod]
        public void TestDirectSend()
        {
            for (int i = 0; i < 1000; i++)
            {
                try
                {
                    //对全体人员广播
                    MqBuilder.CreateBuilder()
                        .withType(MqEnum.Direct)
                        .withReceiver("1111111")
                        .withMessage(new MqMessage
                        {
                            SenderID = "System",
                            MessageID = Guid.NewGuid().ToString("N"),
                            MessageBody = "系统将夜晚0点不停服更新",
                            MessageTitle = "系统提醒",
                        })
                        .SendMessage();
                    MqBuilder.CreateBuilder()
                        .withType(MqEnum.Direct)
                        .withReceiver("2222222")
                        .withMessage(new MqMessage
                        {
                            SenderID = "System",
                            MessageID = Guid.NewGuid().ToString("N"),
                            MessageBody = "系统将夜晚0点不停服更新",
                            MessageTitle = "系统提醒",
                        })
                        .SendMessage();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
