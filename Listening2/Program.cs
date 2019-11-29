using MqSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening2
{
    class Program
    {
        static void Main(string[] args)
        {
            //测试监听2
            MqBuilder.GetInstance()
               .withType(MqEnum.Topic)
               .withListening(MqHelper_Received)
               .Listening();
        }

        private static void MqHelper_Received(object sender, MqMessage e)
        {
            Console.WriteLine(e.MessageBody);
        }
    }
}
