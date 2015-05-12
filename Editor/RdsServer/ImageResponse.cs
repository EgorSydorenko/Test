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
    public class ImageResponse : ServerResponse
    {
        ClientScreenShotData screenShotData;
        ClientScreenShotData tmpShot;

        public ImageResponse(TcpClient client) : base(client) { }

        protected override void Run()
        {
            byte[] bytes = new byte[32768];
            byte[] protocol;
            MemoryStream ms = new MemoryStream();
            NetworkStream ns = client.GetStream();

            var newRgbValue = GetReadyBytes(out protocol);

            this.screenShotData = tmpShot;
            try
            {
                while (true)
                {
                    var length = BitConverter.GetBytes(protocol.Length);
                    ns.Write(length, 0, length.Length);
                    //ns.Read(bytes, 0, bytes.Length);
                    ns.Write(protocol, 0, protocol.Length);
                    //ns.Read(bytes, 0, bytes.Length);
                    ns.Write(newRgbValue.ToArray(), 0, newRgbValue.Count);
                    newRgbValue = GetReadyBytes(out protocol);
                    this.screenShotData = tmpShot;
                    ns.Read(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Соединение разорвано:{0}", ex.Message);
            }
        }

        List<byte> GetReadyBytes(out byte[] btProtocol)
        {
            string answer;
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;
            System.Drawing.Size Coords = Utils.GetRectangleSize(width, height);
            int countHorizontalBlocks = width / Coords.Width;
            int countVerticalBlocks = height / Coords.Height;
            int totalBlockCount = countHorizontalBlocks * countVerticalBlocks;
            var bytes = ImageControler.GetScreenshotBytes();
            this.tmpShot = new ClientScreenShotData(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, bytes.ToArray());

            List<byte> newRgbValue = new List<byte>();
            DateTime start = DateTime.Now;
            if (this.screenShotData != null)
            {
                List<int> blocks = ImageControler.ImageCompressor.getChanges(width, height, Coords.Width, Coords.Height, countHorizontalBlocks, countVerticalBlocks, totalBlockCount, bytes, this.screenShotData);
                var readyBytes = Utils.Compress(ImageControler.ImageCompressor.byteCompressor(blocks, bytes, Coords.Width, Coords.Height, countHorizontalBlocks, countVerticalBlocks, width, height));
                btProtocol = Utils.GetArraysProtocol(blocks, width, height, readyBytes.Length);
                newRgbValue.AddRange(readyBytes);
                Console.WriteLine("{0}ms Length:{1}", (DateTime.Now - start).Milliseconds, newRgbValue.Count);
            }
            else
            {
                var tmp = Utils.Packcage(bytes);
                answer = String.Format("{3}x{0}x{1}x{2}|", width, height, tmp.Count(), "ScreenShot");
                var bts = Encoding.Unicode.GetBytes(answer);
                btProtocol = bts;
                newRgbValue.AddRange(tmp);
            }
            
            return newRgbValue;
        }
    }
}
