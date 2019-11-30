using MqSdk.Entity;
using MqSdk.Utils;
using RabbitMQ.Client;
using System;
using System.Configuration;

namespace MqSdk.Core
{
    internal static class MqConnection
    {

        #region 私有成员

        private static IConnection connection;

        private static object locker = new object();

        #endregion

        #region MQ配置

        private const string FILE = "mqconfig.xml";
        private static MqConfig mqConfig;

        #endregion

        #region MQ连接

        /// <summary>
        /// 初始化连接
        /// </summary>
        static MqConnection()
        {
            //获取配置
            mqConfig = XmlKit.XMLDeSerializer2Object<MqConfig>(FILE);
            //获取连接
            connection = GetMqConnection();
        }

        /// <summary>
        /// 获取MQ连接
        /// </summary>
        /// <returns></returns>
        internal static IConnection GetMqConnection()
        {
            try
            {
                lock (locker)
                {
                    if (connection == null || !connection.IsOpen)
                    {
                        lock (locker)
                        {
                            ConnectionFactory factory = CrateFactory();
                            //此操作耗时，尽量使用一个连接
                            connection = factory.CreateConnection();
                            return connection;
                        }
                    }
                    return connection;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("消息服务初始化失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 创建连接工厂
        /// </summary>
        /// <returns></returns>
        private static ConnectionFactory CrateFactory()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = mqConfig.HostName;
            factory.UserName = mqConfig.UserName;
            factory.Password = EncryptUtility.DesDecrypt(mqConfig.PassWord);
            factory.Port = mqConfig.Port;
            return factory;
        }

        #endregion

    }
}
