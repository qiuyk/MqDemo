using MqSdk.Entity;
using MqSdk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqSdk.Utils
{
    public static class Config
    {
        private const string FILE = "mqconfig.xml";

        private static MqConfig mqConfig;

        public static MqConfig MqConfig
        {
            get
            {
                if (mqConfig == null)
                {
                    mqConfig = XmlKit.XMLDeSerializer2Object<MqConfig>(FILE);
                }
                return mqConfig;
            }
        }
    }
}
