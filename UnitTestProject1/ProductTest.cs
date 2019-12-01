using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MqSdk;
using MqSdk.Entity;

namespace UnitTestProject1
{
    /// <summary>
    /// 测试发送消息
    /// </summary>
    [TestClass]
    public class ProductTest
    {

        #region Fanout 1对多测试
        /// <summary>
        /// 一对多发送消息
        /// 
        /// 场景：发送广播
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

        #endregion

        #region Direct 1对1测试

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
                            SenderID = "XXXXXXX",
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

        #endregion

        #region Topic 分组测试

        /// <summary>
        /// 分组测试
        /// 
        /// 场景：通知软件部门所有成员下班厕所见
        /// </summary>
        [TestMethod]
        public void TestTopicSend01()
        {
            for (int i = 0; i < 1000; i++)
            {
                try
                {
                    //对全体人员广播
                    MqBuilder.CreateBuilder()
                        .withType(MqEnum.Topic)
                        .withRole("soft")
                        .withMessage(new MqMessage
                        {
                            SenderID = "Boss",
                            MessageID = Guid.NewGuid().ToString("N"),
                            MessageBody = "软件部门所有成员下班厕所见",
                            MessageTitle = "老板来信",
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
        /// 分组测试
        /// 
        /// 场景：通知财务部门所有成员下班办公室见
        /// </summary>
        [TestMethod]
        public void TestTopicSend02()
        {
            for (int i = 0; i < 500; i++)
            {
                try
                {
                    //对全体人员广播
                    MqBuilder.CreateBuilder()
                        .withType(MqEnum.Topic)
                        .withRole("finance")
                        .withMessage(new MqMessage
                        {
                            SenderID = "Boss",
                            MessageID = Guid.NewGuid().ToString("N"),
                            MessageBody = "软件部门所有成员下班厕所见",
                            MessageTitle = "老板来信",
                        })
                        .SendMessage();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        #endregion
    }
}
