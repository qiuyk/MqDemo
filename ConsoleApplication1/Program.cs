using MqSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening1
{
    class Program
    {
        static void Main(string[] args)
        {
           
            //测试监听1
            MqBuilder.GetInstance()
               .withReceiver("1111111")
               .withListening(MqHelper_Received)
               .Listening();

        }

        private static void MqHelper_Received(object sender, MqMessage e)
        {
            Console.WriteLine(e.SenderID + "来信：" + e.MessageBody);
        }
    }
}
