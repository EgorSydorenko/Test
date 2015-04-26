using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkWithImage;

namespace RdsServer
{
    public class PreviewResponse : ServerResponse
    {

        public delegate void EndConnectionEvent();

        public event EndConnectionEvent ConnectEvent;

        public PreviewResponse(TcpClient client) : base(client) { }

        protected override void Run()
        {
            byte[] bytes = new byte[32768];

            MemoryStream ms = new MemoryStream();
            NetworkStream ns = client.GetStream();
            byte[] protocol;
            List<byte> newRgbValue;

            //this.screenShotData = tmpShot;
            try
            {
                while (true)
                {

                    ms.SetLength(0);
                    do
                    {
                        int cnt = ns.Read(bytes, 0, bytes.Length);
                        if (cnt == 0)
                        {
                            throw new Exception("Получено 0 байт");
                        }
                        ms.Write(bytes, 0, cnt);
                    } while (client.Available > 0);
                    newRgbValue = GetReadyBytes(out protocol);
                    String msg = Encoding.Unicode.GetString(ms.GetBuffer(), 0, (int)ms.Length);
                    var bt = Convert.ToByte(protocol.Length);
                    ns.WriteByte(bt);
                    //System.Threading.Thread.Sleep(200);
                    ns.Write(protocol, 0, protocol.Length);
                    //System.Threading.Thread.Sleep(200);
                    ns.Write(newRgbValue.ToArray(), 0, newRgbValue.Count);
                    //ns.Write(newRgbValue.ToArray(), 0, newRgbValue.Count/2);
                    //System.Threading.Thread.Sleep(200);
                    //ns.Write(newRgbValue.ToArray(), newRgbValue.Count / 2, newRgbValue.Count - newRgbValue.Count / 2);
                    newRgbValue.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Соединение разорвано:{0}", ex.Message);
                this.ConnectEvent();
            }
        }

        List<byte> GetReadyBytes(out byte[] protocol)
        {
            string answer;
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;
            //var bytes = ImageControler.GetScreenshotBytes(150,150);
            var bytes = ImageControler.GetScreenshotBytes();
            List<byte> newRgbValue = new List<byte>();
            var tmp = Utils.Packcage(bytes);
            answer = String.Format("{3}x{0}x{1}x{2}|", width, height, tmp.Count(), "ScreenShot");
            protocol = Encoding.Unicode.GetBytes(answer);
            //newRgbValue.AddRange(bts);
            //newRgbValue.AddRange(tmp);
            return tmp;
        }
    }
}
