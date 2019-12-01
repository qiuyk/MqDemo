using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MqSdk;
using MqSdk.Entity;
using System.Threading;
using System.Collections.Generic;

namespace UnitTestProject1
{
    /// <summary>
    /// 监听测试
    /// </summary>
    [TestClass]
    public class CustomerTest
    {

        #region Fanout 1对多测试
        /// <summary>
        /// 一对多监听 接收者1
        /// 
        /// 场景：接收广播
        /// </summary>
        [TestMethod]
        public void TestFanoutListening01()
        {
            
            try
            {
                MqBuilder.CreateBuilder()
                    .withType(MqEnum.Fanout)
                    .withReceiver("1111111")
                    .withListening(MqMessage_Received)
                    .Listening();
                Thread.Sleep(3*1000);
                Console.WriteLine();
                list.ForEach(s =>
                {
                    Assert.AreNotEqual(s, null);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 一对多监听 接收者2
        /// 
        /// 场景：接收广播
        [TestMethod]
        public void TestFanoutListening02()
        {

            try
            {
                MqBuilder.CreateBuilder()
                    .withType(MqEnum.Fanout)
                    .withReceiver("2222222")
                    .withListening(MqMessage_Received)
                    .Listening();
                Thread.Sleep(3 * 1000);
                Console.WriteLine();
                list.ForEach(s =>
                {
                    Assert.AreNotEqual(s, null);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

        #region Direct 1对1测试
        /// <summary>
        /// 1对1监听 接收者1
        /// 
        /// 场景：接收广播
        /// </summary>
        [TestMethod]
        public void TestDirectListening01()
        {

            try
            {
                MqBuilder.CreateBuilder()
                    .withType(MqEnum.Direct)
                    .withReceiver("1111111")
                    .withListening(MqMessage_Received)
                    .Listening();
                Thread.Sleep(3 * 1000);
                Console.WriteLine();
                list.ForEach(s =>
                {
                    Assert.AreNotEqual(s, null);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// 1对1监听 接收者1
        /// 
        /// 场景：接收广播
        /// </summary>
        [TestMethod]
        public void TestDirectListening02()
        {

            try
            {
                MqBuilder.CreateBuilder()
                    .withType(MqEnum.Direct)
                    .withReceiver("2222222")
                    .withListening(MqMessage_Received)
                    .Listening();
                Thread.Sleep(3 * 1000);
                Console.WriteLine();
                list.ForEach(s =>
                {
                    Assert.AreNotEqual(s, null);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

        #region Topic 分组广播
        /// <summary>
        /// 分组监听 接收者1
        /// 
        /// 场景：通知软件部门所有成员下班厕所见
        /// </summary>
        [TestMethod]
        public void TestTopicListening01()
        {

            try
            {
                MqBuilder.CreateBuilder()
                    .withType(MqEnum.Topic)
                    .withRole("soft")
                    .withReceiver("1111111")
                    .withListening(MqMessage_Received)
                    .Listening();
                Thread.Sleep(3 * 1000);
                Console.WriteLine();
                list.ForEach(s =>
                {
                    Assert.AreNotEqual(s, null);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// 分组监听 接收者2
        /// 
        /// 场景：通知软件部门所有成员下班厕所见
        /// </summary>
        [TestMethod]
        public void TestTopicListening02()
        {

            try
            {
                MqBuilder.CreateBuilder()
                    .withType(MqEnum.Topic)
                    .withRole("soft")
                    .withReceiver("2222222")
                    .withListening(MqMessage_Received)
                    .Listening();
                Thread.Sleep(3 * 1000);
                Console.WriteLine();
                list.ForEach(s =>
                {
                    Assert.AreNotEqual(s, null);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 分组监听 接收者3
        /// 
        /// 场景：通知财务部门所有成员下班办公室见
        /// </summary>
        [TestMethod]
        public void TestTopicListening03()
        {

            try
            {
                MqBuilder.CreateBuilder()
                    .withType(MqEnum.Topic)
                     .withRole("finance")
                    .withReceiver("3333333")
                    .withListening(MqMessage_Received)
                    .Listening();
                Thread.Sleep(3 * 1000);
                Console.WriteLine();
                list.ForEach(s =>
                {
                    Assert.AreNotEqual(s, null);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

        #region 消息接收

        private static List<MqMessage> list = new List<MqMessage>();

        private static void MqMessage_Received(object sender, MqMessage e)
        {
            list.Add(e);
        }

        #endregion

        #region 异常处理测试

        /// <summary>
        /// 异步异常处理
        /// </summary>
        [TestMethod]
        public void TestAsyncListeningException()
        {
            for (int i = 0; i < 500; i++)
            {
                try
                {
                    MqBuilder.CreateBuilder()
                             .withType(MqEnum.Topic)
                             .withListening(MqMessage_Received)
                             .withAsyncException(MqException_Received)
                             .SendMessageAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private static void MqException_Received(object sender, Exception e)
        {
            //测方法为异步返回，请注意处理方式
        }

        #endregion
    }
}
