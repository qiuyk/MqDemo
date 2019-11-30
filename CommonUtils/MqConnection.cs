using RabbitMQ.Client;
using System;
using System.Configuration;

namespace MqSdk
{
    internal static class MqConnection
    {

        #region 私有成员

        private static IConnection connection;

        private static object locker = new object();

        #endregion

        #region 上线配置
        //private static string HOSTNAME = ConfigurationManager.AppSettings["MqHostName"].ToString();
        //private static string USERNAME = ConfigurationManager.AppSettings["MqUserName"].ToString();
        //private static string PASSWORD = EncryptUtility.DesDecrypt("ConfigurationManager.AppSettings["MqPassWord"].ToString());
        //private static string PORT = ConfigurationManager.AppSettings["MqPort"].ToString();
        #endregion

        #region 测试配置
        private static string HOSTNAME = "localhost";
        private static string USERNAME = "test";
        private static string PASSWORD = EncryptUtility.DesDecrypt("08A98110DD4D9427");
        private static string PORT = "5672";
        #endregion

        #region MQ连接
        /// <summary>
        /// 初始化连接
        /// </summary>
        static MqConnection()
        {
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
            factory.HostName = MqConnection.HOSTNAME;
            factory.UserName = MqConnection.USERNAME;
            factory.Password = MqConnection.PASSWORD;
            int port;
            if (int.TryParse(MqConnection.PORT, out port))
            {
                factory.Port = port;
            }
            else
            {
                throw new Exception("MQ端口异常");
            }
            return factory;
        }

        #endregion

    }
}
