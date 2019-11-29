using RabbitMQ.Client;
using System;
using System.Configuration;

namespace MqSdk
{
    internal static class MqConnection
    {
        private static IConnection connection;

        public static string HOSTNAME = ConfigurationManager.AppSettings["MqHostName"].ToString();
        public static string USERNAME = ConfigurationManager.AppSettings["MqUserName"].ToString();
        public static string PASSWORD = ConfigurationManager.AppSettings["MqPassWord"].ToString();
        public static string PORT = ConfigurationManager.AppSettings["MqPort"].ToString();
        static MqConnection()
        {
            try
            {
                ConnectionFactory factory = CrateFactory();
                //此操作耗时，尽量使用一个连接
                connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                throw new Exception("消息服务初始化失败：" + ex.Message);
            }
        }

        internal static IConnection GetMqConnection()
        {
            lock (connection)
            {
                if (connection == null || !connection.IsOpen)
                {
                    lock (connection)
                    {
                        ConnectionFactory factory = CrateFactory();
                        connection = factory.CreateConnection();
                        return connection;
                    }
                }
                return connection;
            }
        }

        private static ConnectionFactory CrateFactory()
        {
            //初始化连接资源
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
    }
}
