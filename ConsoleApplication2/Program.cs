using MqSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product
{
    class Program
    {
        static void Main(string[] args)
        {
            MqBuilder.GetInstance()
                .withMessage(new MqMessage
                {
                    MessageID = "123",
                    MessageBody = "1111111",
                    MessageTitle = "",
                })
                .SendMessage();
        }
    }
}
