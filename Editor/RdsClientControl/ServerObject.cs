using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdsClient
{
    public class ServerObject
    {
        public string ipAddress { set; get; }
        public int imagePort { set; get; }
        public int lowImagePort { set; get; }
        public int controlPort { set; get; }


        public ServerObject(string msg)
        {
            var msgData = msg.Split('|');
            this.ipAddress = msgData[0].Split(':')[1];
            this.imagePort = Convert.ToInt32(msgData[1].Split(':')[1]);
            this.controlPort = Convert.ToInt32(msgData[2].Split(':')[1]);
            this.lowImagePort = Convert.ToInt32(msgData[3].Split(':')[1]);
        }
    }
}
