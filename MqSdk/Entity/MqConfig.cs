using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqSdk.Entity
{
    [Serializable]
    public class MqConfig
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int Port { get; set; }
        public string AppID { get; set; }
    }
}
