using MqSdk;
using MqSdk.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Product
{
    class Program
    {
        /// <summary>
        /// 生产者
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {

                    MqBuilder.CreateBuilder()
                   .withMessage(new MqMessage
                   {
                       SenderID = "XXOOXXO",
                       MessageID = "1111111",
                       MessageBody = "你好1111111",
                       MessageTitle = "",
                   })
                   .withReceiver("1111111")
                   .SendMessage();
                    Thread.Sleep(1000);

                    MqBuilder.CreateBuilder()
                    .withMessage(new MqMessage
                    {
                        SenderID = "XXOOXXO",
                        MessageID = "2222222",
                        MessageBody = "你好2222222",
                        MessageTitle = "",
                    })
                    .withReceiver("2222222")
                    .SendMessage();
                    Thread.Sleep(1000);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            
        }
    }
}
