using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RdsServer
{
    public class Server
    {
        TcpListener imageListener;
        TcpListener controlListener;
        TcpListener lowImageListener;
        IPAddress ipAddress;
        UdpClient sonar;
        Dictionary<string, int> broadCastPorts { set; get; }

        #region Threads

        Task imagesThread;
        Task controlsThread;
        Task udpThread;
        Task lowImageThread;

        #endregion

        bool udpStop = false;

        public Server()
        {
            this.broadCastPorts = new Dictionary<string, int>();
            this.broadCastPorts.Add("imagePort", Convert.ToInt32(ConfigurationManager.AppSettings["imagePort"].ToString()));
            this.broadCastPorts.Add("controlPort", Convert.ToInt32(ConfigurationManager.AppSettings["controlPort"].ToString()));
            this.broadCastPorts.Add("lowImagePort", Convert.ToInt32(ConfigurationManager.AppSettings["lowImagePort"].ToString()));
            this.broadCastPorts.Add("udpPort", Convert.ToInt32(ConfigurationManager.AppSettings["udpPort"].ToString()));

            this.ipAddress = IPAddress.Parse(ConfigurationManager.AppSettings["ipAddress"]);

            sonar = new UdpClient(ConfigurationManager.AppSettings["udpIp"], broadCastPorts["udpPort"]);
            this.imageListener = new TcpListener(ipAddress, broadCastPorts["imagePort"]);
            this.controlListener = new TcpListener(ipAddress, broadCastPorts["controlPort"]);
            this.lowImageListener = new TcpListener(ipAddress, broadCastPorts["lowImagePort"]);

            this.udpThread = new Task(UdpBroadCasting);
            this.imagesThread = new Task(ImageListenerServer);
            this.lowImageThread = new Task(lowPicturesListerServer);
            this.controlsThread = new Task(ControlListenerServer);
            //udpThread.Start();
        }


        public void StartServer()
        {
            udpThread.Start();
            imagesThread.Start();
            controlsThread.Start();
            lowImageThread.Start();
        }

        private void UdpBroadCasting()
        {
            var message = Encoding.Unicode.GetBytes(String.Format("Ip:{0}|imagePort:{1}|controlPort:{2}|lowImagePort:{3}", this.ipAddress.ToString(), broadCastPorts["imagePort"], broadCastPorts["controlPort"], broadCastPorts["lowImagePort"]));
            try
            {
                while (!udpStop)
                {
                    this.sonar.Send(message, message.Count());
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ImageListenerServer()
        {
            this.imageListener.Start();
            while (true)
            {
                var client = imageListener.AcceptTcpClient();
                this.udpStop = true;
                (new ImageResponse(client)).StartResponse();
            }
        }

        private void ControlListenerServer()
        {
            this.controlListener.Start();
            while (true)
            {
                var client = controlListener.AcceptTcpClient();
                this.udpStop = true;
                (new ControlResponse(client)).StartResponse();
            }
        }

        private void lowPicturesListerServer()
        {
            this.lowImageListener.Start();
            while (true)
            {
                var client = lowImageListener.AcceptTcpClient();
                this.udpStop = true;
                var previewClient = (new PreviewResponse(client));
                previewClient.ConnectEvent += PreviewClient_ConnectEvent;
                previewClient.StartResponse();
                //(new ServerResponse(client, ConfigurationManager.AppSettings["ipAddress"], port + 1)).Start();
                ////Stop UDP if server find client
                //udpSendingThread.Abort();
            }
        }

        private void PreviewClient_ConnectEvent()
        {
            udpStop = false;
            udpThread = new Task(UdpBroadCasting);
            udpThread.Start();
        }
    }
}
