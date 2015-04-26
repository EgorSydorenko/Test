using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RdsServer
{
    class ControlResponse : ServerResponse
    {
        public ControlResponse(TcpClient client) : base(client) { }

        protected override void Run()
        {
            byte[] bytes = new byte[32768];

            MemoryStream ms = new MemoryStream();
            NetworkStream ns = client.GetStream();

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
                    String msg = Encoding.Unicode.GetString(ms.GetBuffer(), 0, (int)ms.Length);
                    this.Answer(msg, ns);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Соединение разорвано:{0}", ex.Message);
            }
        }

        private void Answer(string msg, NetworkStream ntStream)
        {
            var msgData = msg.Split('|');
            byte[] answerByts;
            Point point;
            int keyCode;
            switch (msgData[0])
            {
                case "LUP":
                    point = RemoteDesktopControl.GetCoordsToCLick(msgData[1]);
                    RemoteDesktopControl.MouseUp(point, MouseEvent.MOUSEEVENTF_LEFTUP);
                    break;
                case "RUP":
                    point = RemoteDesktopControl.GetCoordsToCLick(msgData[1]);
                    RemoteDesktopControl.MouseUp(point, MouseEvent.MOUSEEVENTF_RIGHTUP);
                    break;
                case "LDOWN":
                    point = RemoteDesktopControl.GetCoordsToCLick(msgData[1]);
                    RemoteDesktopControl.MouseDown(point, MouseEvent.MOUSEEVENTF_LEFTDOWN);
                    break;
                case "RDOWN":
                    point = RemoteDesktopControl.GetCoordsToCLick(msgData[1]);
                    RemoteDesktopControl.MouseDown(point, MouseEvent.MOUSEEVENTF_RIGHTDOWN);
                    break;
                case "Move":
                    point = RemoteDesktopControl.GetCoordsToCLick(msgData[1]);
                    RemoteDesktopControl.MouseMove(point);
                    break;
                case "KEYDOWN":
                    keyCode = RemoteDesktopControl.GetKeyCode(msgData[1]);
                    RemoteDesktopControl.KeyboardEventDown(keyCode);
                    break;
                case "KEYUP":
                    keyCode = RemoteDesktopControl.GetKeyCode(msgData[1]);
                    RemoteDesktopControl.KeyboardEventUp(keyCode);
                    break;
                case "OK":
                    return;
                case "REBOOT":
                    RemoteDesktopControl.WindowsManagment(WindowsManagmentKeys.enReboot);
                    break;
                case "SHUTDOWN":
                    RemoteDesktopControl.WindowsManagment(WindowsManagmentKeys.enShutDown);
                    break;
                case "LOGOFF":
                    RemoteDesktopControl.WindowsManagment(WindowsManagmentKeys.enLogOff);
                    break;
            }
            answerByts = Encoding.Unicode.GetBytes("OK");
            ntStream.Write(answerByts, 0, answerByts.Length);
        }

        
    }
}
