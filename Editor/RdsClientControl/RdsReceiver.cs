using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RdsClient
{
    public class RdsReceiver
    {
        UdpClient receiver;
        IPEndPoint udpEndPoint;


        Thread udpListener;
        Dictionary<string, RemoteServer> servers;

        public RdsReceiver(Dictionary<string, RemoteServer> servers)
        {
            receiver = new UdpClient(Int32.Parse(ConfigurationManager.AppSettings["udpPort"]));
            udpEndPoint = new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["udpIp"]), Int32.Parse(ConfigurationManager.AppSettings["udpPort"]));
            udpListener = new Thread(RunUdp);
            udpListener.IsBackground = true;
            udpListener.SetApartmentState(ApartmentState.STA);
            this.servers = servers;
        }


        public void StartListen()
        {
            this.udpListener.Start();
        }


        private void RunUdp()
        {
            try
            {
                int count = 0;
                while (true)
                {
                    byte[] a = receiver.Receive(ref udpEndPoint);
                    string msg = Encoding.Unicode.GetString(a, 0, a.Length);
                    var server = new RemoteServer(msg);
                    if (!this.servers.ContainsKey(server.ipAddress))
                    {
                        servers.Add(server.ipAddress, server);
                        server.StartPreview();
                        server.Number = count;
                        count++;
                    }
                }
            }
            catch { }

        }



    }
}
