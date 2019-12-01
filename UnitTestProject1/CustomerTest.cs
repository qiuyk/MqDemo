using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MqSdk;
using MqSdk.Entity;
using System.Threading;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class CustomerTest
    {
        private static List<MqMessage> list = new List<MqMessage>();
        /// <summary>
        /// 一对多监听 接受者1
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
                    .withListening(MqHelper_Received)
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
        /// 一对多监听 接受者2
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
                    .withListening(MqHelper_Received)
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
        /// 1对1监听 接受者1
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
                    .withListening(MqHelper_Received)
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
        /// 1对1监听 接受者1
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
                    .withListening(MqHelper_Received)
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


        private static void MqHelper_Received(object sender, MqMessage e)
        {
            list.Add(e);
        }

    }
}
