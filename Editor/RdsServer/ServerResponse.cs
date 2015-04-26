using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RdsServer
{
    public abstract class ServerResponse
    {
        protected TcpClient client;

        protected Task responseThread;

        public void StartResponse()
        {
            responseThread.Start();
        }

        abstract  protected void Run();

         public ServerResponse(TcpClient client)
        {
            this.client = client;
            this.responseThread = new Task(Run);
        }

    }
}
