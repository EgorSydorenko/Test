using RdsClientControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WorkWithImage;

namespace RdsClient
{
    public class RemoteServer
    {
        public string ipAddress { private set; get; }
        public int imagePort { private set; get; }
        public int controlPort { private set; get; }
        public int lowImagePort { private set; get; }

        public static int index;
        public static SortedList<string, int> ipAdresses ;

        private static object slockObject;

        public int Number { set; get; }

        TcpClient lowPicsClient;
        TcpClient controlsClient;
        TcpClient imageClient;

        Thread imageTask;
        Thread lowPicsTask;
        Thread controlTask;
        string message;
        System.Windows.Controls.Image img;

        object lockObject = new object();
        public ClientScreenShotData data { private set; get; }
        //Image img;

        static RemoteServer() 
        {
            ipAdresses = new SortedList<string, int>();
            slockObject = new object();
        }


        public RemoteServer(string msg)
        {
            try
            {
                var msgData = msg.Split('|');
                ipAddress = msgData[0].Split(':')[1];
                imagePort = Convert.ToInt32(msgData[1].Split(':')[1]);
                controlPort = Convert.ToInt32(msgData[2].Split(':')[1]);
                lowImagePort = Convert.ToInt32(msgData[3].Split(':')[1]);
               
                //

                imageTask = new Thread(RunImages);
                lowPicsTask = new Thread(RunLowPics);
                imageTask.IsBackground = true;
                imageTask.SetApartmentState(ApartmentState.STA);
                
                lowPicsTask.IsBackground=true;
                lowPicsTask.SetApartmentState(ApartmentState.STA);
                controlTask = new Thread(RunControl);
                this.controlTask.IsBackground = true;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void StartPreview()
        {
            try
            {
                this.lowPicsClient = new TcpClient(ipAddress, lowImagePort);
                lowPicsTask.Start();
                RdsControl.ImageContainer.Dispatcher.Invoke(new Action(() =>
                {
                    //MainWindow.Images[this.Number].Tag = this.ipAddress;
                    //Border bd = new Border();
                    //bd.BorderThickness = 2;
                    //bd.BorderBrush = 

                    Grid grid = new Grid();
                    grid.Margin = new Thickness(5);
                    grid.Background = System.Windows.Media.Brushes.Black;
                    grid.Cursor = System.Windows.Input.Cursors.Hand;
                    //grid.ShowGridLines = true;
                    grid.Height = 230;
                    grid.Width = 250;
                    grid.Tag = this.ipAddress;

                    RowDefinition gridRow2 = new RowDefinition();
                    gridRow2.Height = new GridLength(1, GridUnitType.Star);
                    RowDefinition gridRow1 = new RowDefinition();
                    gridRow1.Height = new GridLength(30);
                    //RowDefinition gridRow3 = new RowDefinition();
                    //gridRow3.Height
                    grid.RowDefinitions.Add(gridRow1);
                    grid.RowDefinitions.Add(gridRow2);
                    Border bd = new Border();
                    bd.Background = System.Windows.Media.Brushes.White;
                    bd.BorderThickness = new Thickness(2);
                    //bd.Width = 246;
                    //bd.Height = 30;
                    #region Canvas Panel
                    Canvas canvas = new Canvas();
                    canvas.Height = 30;
                    canvas.Width = 250;
                    canvas.Background = System.Windows.Media.Brushes.LightGray;
                    canvas.Opacity = 0.6;
                    canvas.VerticalAlignment = VerticalAlignment.Bottom;
                    canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
                    Canvas.SetZIndex(canvas, 2);
                    IntPtr hBmp = Images.Restart_50.GetHbitmap();
                    System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    img.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                    Utils.DeleteObject(hBmp);
                    img.Width = 24;
                    img.Height = 24;
                    img.Tag = this.ipAddress;
                    img.Style = (Style)RdsControl.MainWind.FindResource("Hover");
                    Canvas.SetZIndex(img, 3);
                    Canvas.SetLeft(img, 15);
                    Canvas.SetTop(img, 3);
                    //img.FindResource("Hover");
                    img.MouseDown += RdsControl.MainWind.RebootClick;
                    canvas.Children.Add(img);

                    IntPtr hBmp1 = Images.Shutdown_50.GetHbitmap();
                    System.Windows.Controls.Image img1 = new System.Windows.Controls.Image();
                    img1.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp1, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                    Utils.DeleteObject(hBmp1);
                    img1.Width = 24;
                    img1.Height = 24;
                    img1.Style = (Style)RdsControl.MainWind.FindResource("Hover");
                    img1.Tag = this.ipAddress;
                    img1.MouseDown += RdsControl.MainWind.ShutDownClick;


                    Canvas.SetZIndex(img1, 3);
                    Canvas.SetRight(img1, 15);
                    Canvas.SetTop(img1, 3);
                    canvas.Children.Add(img1);


                    IntPtr hBmp2 = Images.logout.GetHbitmap();
                    System.Windows.Controls.Image img2 = new System.Windows.Controls.Image();
                    img2.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp2, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                    Utils.DeleteObject(hBmp2);
                    img2.Width = 24;
                    img2.Height = 24;
                    img2.Style = (Style)RdsControl.MainWind.FindResource("Hover");
                    img2.Tag = this.ipAddress;
                    img2.MouseDown += RdsControl.MainWind.LogoutClick;


                    Canvas.SetZIndex(img2, 3);
                    Canvas.SetRight(img2, 113);
                    Canvas.SetTop(img2, 3);
                    canvas.Children.Add(img2);
                    #endregion
                    Label lbl = new Label();
                    lbl.Content = String.Format("IpAddress: {0}", this.ipAddress);
                    lbl.VerticalAlignment = VerticalAlignment.Center;
                    lbl.HorizontalAlignment = HorizontalAlignment.Center;
                    lbl.FontWeight = FontWeights.Bold;
                    lbl.FontSize = 16;
                    Canvas.SetZIndex(lbl, 2);
                    Canvas.SetZIndex(bd, 1);
                    Grid.SetRow(lbl, 0);
                    grid.Children.Add(lbl);
                    Grid.SetRow(bd, 0);
                    grid.Children.Add(bd);
                    System.Windows.Controls.Image someImg = new System.Windows.Controls.Image();
                    someImg.Width = 250;
                    someImg.Height = 200;
                    someImg.HorizontalAlignment = HorizontalAlignment.Center;
                    someImg.VerticalAlignment = VerticalAlignment.Center;
                    someImg.Tag = this.ipAddress;
                    someImg.Stretch = System.Windows.Media.Stretch.UniformToFill;
                    this.img = someImg;
                    someImg.MouseDown += RdsControl.MainWind.ImageClick;
                    Grid.SetRow(someImg, 1);
                    Grid.SetRow(canvas, 1);
                    grid.Children.Add(someImg);
                    grid.Children.Add(canvas);
                    int val = 0;
                    lock (slockObject)
                    {
                        try
                        {
                            Int32.TryParse(this.ipAddress.Split(".".ToArray(), StringSplitOptions.RemoveEmptyEntries).Last(), out val);
                            RemoteServer.ipAdresses.Add(this.ipAddress, val);
                        }
                        catch (Exception ex)
                        {
                            if (RemoteServer.ipAdresses[this.ipAddress] != null)
                                {
                                   //if(RemoteServer.ipAdresses.Keys.Contains())
                                    RemoteServer.ipAdresses.Remove(this.ipAddress);
                                    RemoteServer.ipAdresses.Add(this.ipAddress, val);
                                }
                            
                        }
                        //RdsControl.ImageContainer.Children.Add(grid);

                        if (ipAdresses[ipAddress] <= ipAdresses.Count)
                        {
                            RdsControl.ImageContainer.Children.Insert(ipAdresses[ipAddress], grid);
                        }
                        else
                        {
                            RdsControl.ImageContainer.Children.Add(grid);
                        }
                        
                    }



                    

                }));
            }
            catch { }

        }


        public void StartControl(string msg)
        {
            lock (lockObject)
            {
                this.message = msg;
            }
        }

        public void StartView()
        {
            this.imageClient = new TcpClient(ipAddress, imagePort);
            //var result = this.imageClient.BeginConnect(ipAddress, imagePort,null,null);
            //var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
            //if (!success)
            //{
            //    Console.WriteLine("Соединение не установлено");
            //    throw new Exception("Connection Timeout");
            //}
            //Console.WriteLine("Соединение установлено");
           // imageClient.SendTimeout = 10000;
            //imageClient.ReceiveTimeout
            this.controlsClient = new TcpClient(ipAddress, controlPort);

            imageTask.Start();
            controlTask.Start();
        }

        void RunLowPics()
        {
            byte[] bytes;
            MemoryStream ms = new MemoryStream();
            NetworkStream ns = lowPicsClient.GetStream();
            var msg = Encoding.Unicode.GetBytes("PREVIEW");
            try
            {
                while (true)
                {
                    ms.SetLength(0);
                    ns.Write(msg, 0, msg.Length);
                    int length = ns.ReadByte();
                    bytes = new byte[length];
                    ns.Read(bytes, 0, bytes.Length);
                    var firstPackage = Encoding.Unicode.GetString(bytes);
                    Console.WriteLine(firstPackage);
                    var protocol = firstPackage.Substring(0, firstPackage.IndexOf('|'));
                    var protocolInfo = protocol.Split('x');
                    string commad = protocolInfo[0];
                    int widthResolution = Convert.ToInt32(protocolInfo[1]);
                    int heightResolution = Convert.ToInt32(protocolInfo[2]);
                    long fullPackageLength = Convert.ToInt64(protocolInfo[3]);
                    var fileInfo = firstPackage.Substring(firstPackage.IndexOf('|') + 1);
                    var cl_data = new ClientScreenShotData(widthResolution, heightResolution,
                    Utils.UnPacking(GetBytes(0, ns, ms, fullPackageLength, lowPicsClient)));
                    img.Dispatcher.Invoke(new Action(() =>
                    {
                        img.Source = ImageControler.ImageDeCompressor.CreateNewImage
                        (
                            cl_data
                        );
                    }));
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                try
                {
                    RdsControl.SetMainImageNewPic.Dispatcher.Invoke(new Action(() =>
                    {
                        RdsControl.MainWind.DeletePicture(ipAddress);
                        RdsControl.MainWind.CloseOutside();
                    }));
                    StopViewControl();
                }
                catch { }
            }
        }

        void RunControl()
        {

            try
            {
                NetworkStream ns = this.controlsClient.GetStream();
                MemoryStream ms = new MemoryStream();
                byte[] answer = new byte[32768];
                while(true)
                {
                    if (!String.IsNullOrEmpty(message))
                    {
                        var bytes = Encoding.Unicode.GetBytes(message);
                        this.message = string.Empty;
                        ns.Write(bytes, 0, bytes.Length);
                        ms.SetLength(0);
                        int count = ns.Read(answer, 0, answer.Length);
                        var firstPackage = Encoding.Unicode.GetString(answer).Substring(0, count);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
           

        }

        public void ShutDownOrReboot(string message)
        {
            try
            {
                this.controlsClient = new TcpClient(ipAddress, controlPort);
                NetworkStream ns = this.controlsClient.GetStream();
                MemoryStream ms = new MemoryStream();
                byte[] answer = new byte[32768];

                if (!String.IsNullOrEmpty(message))
                {
                    var bytes = Encoding.Unicode.GetBytes(message);
                    this.message = string.Empty;
                    ns.Write(bytes, 0, bytes.Length);
                    ms.SetLength(0);
                }
            }
            catch { }
            finally
            {
                this.controlsClient = null;
            }
               
        }

        private byte[] GetBytes(long allBytes, NetworkStream ns, MemoryStream ms, long fullPackageLength,TcpClient client,int btSize = 32768 )
        {
            byte[] bytes = new byte[32768];
            Console.WriteLine("First bytes {0}",allBytes);
            try
            {
                while (fullPackageLength > allBytes)
                {
                    //do
                    //{
                        int cnt = ns.Read(bytes, 0, bytes.Length);
                        
                        if (cnt == 0)
                        {
                            throw new Exception("Получено 0 байт");
                        }
                        ms.Write(bytes, 0, cnt);
                        allBytes += cnt;
                       //Console.WriteLine("AllBytes {0}, cnt {1}, fullPackageLength {2}",allBytes,cnt ,fullPackageLength);
                    //} while (client.Available > 0);
                }
               //Console.WriteLine("Exit");
            }
            catch (Exception ex) 
            { Console.WriteLine(ex.Message); }
            byte[] fileBytes = new byte[ms.Length];
            ms.Position = 0;
            var answ = ms.Read(fileBytes, 0, (int)ms.Length);
            ms.SetLength(0);
            return fileBytes;
        }

        private byte[] GetProtocol(NetworkStream ns, MemoryStream ms, long fullPackageLength, TcpClient client, int btSize)
        {
            byte[] bytes = new byte[4];
            int allBytes = 0;
            try
            {
                while (allBytes < fullPackageLength)
                {
                    //do
                    //{
                    int cnt = ns.Read(bytes, 0, bytes.Length);

                    if (cnt == 0)
                    {
                        throw new Exception("Получено 0 байт");
                    }
                    ms.Write(bytes, 0, cnt);
                    allBytes += cnt;
                    //Console.WriteLine("AllBytes {0}, cnt {1}, fullPackageLength {2}", allBytes, cnt, fullPackageLength);
                    //} while (client.Available > 0);
                }
                //Console.WriteLine("Exit");
            }
            catch (Exception ex)
            { Console.WriteLine(); }
            byte[] fileBytes = new byte[ms.Length];
            ms.Position = 0;
            var answ = ms.Read(fileBytes, 0, (int)ms.Length);
            ms.SetLength(0);
            return fileBytes;
        }

        void RunImages()
        {
            NetworkStream ns = this.imageClient.GetStream();
            MemoryStream ms = new MemoryStream();
            byte[] answer;
            byte[] lg = new byte[8];
            BitmapSource source = null;
            var answerBts = Encoding.Unicode.GetBytes("OK");
            try
            {
                while (true)
                {
                    
                    ms.SetLength(0);
                    int count = ns.Read(lg, 0, lg.Length);
                    
                    //ns.Write(answerBts, 0, answerBts.Length);
                    var protocolLength = BitConverter.ToInt32(lg,0);

                    //Regex reg = new Regex("^([0-9]+)", RegexOptions.Multiline);
                    //var match = reg.Match(str, 0);
                    //v Convert.ToInt32(match.Value);
                    //answer = new byte[protocolLength];
                    ms.SetLength(0);
                    answer = GetProtocol(ns, ms, protocolLength, imageClient, protocolLength);
                    //ns.Write(answerBts, 0, answerBts.Length);
                    long allBytes = 0;
                    var firstPackage = Encoding.Unicode.GetString(answer);
                    var protocol = firstPackage.Substring(0, firstPackage.IndexOf('|'));
                    var protocolInfo = protocol.Split('x');
                    string commad = protocolInfo[0];
                    int widthResolution = Convert.ToInt32(protocolInfo[1]);
                    int heightResolution = Convert.ToInt32(protocolInfo[2]);
                    long fullPackageLength = Convert.ToInt64(protocolInfo[3]);


                    if (protocolInfo.Count() == 4)
                    {
                        var fileInfo = firstPackage.Substring(firstPackage.IndexOf('|') + 1);
                        var packegeBytes = Encoding.Unicode.GetBytes(fileInfo);
                        ms.Write(packegeBytes, 0, packegeBytes.Length);
                        allBytes = packegeBytes.Length;
                        var fileBytes = Utils.UnPacking(GetBytes(allBytes, ns, ms, fullPackageLength, imageClient));
                        data = new ClientScreenShotData(widthResolution, heightResolution, fileBytes);
                    }
                    else
                    {
                        List<int> blocks;
                        if (protocolInfo[4] != "[]")
                            blocks = Utils.GetRectNumbs(protocolInfo[4]);
                        else
                            blocks = null;
                        var fileInfo = firstPackage.Substring(firstPackage.IndexOf('|') + 1);
                        var packegeBytes = Encoding.Unicode.GetBytes(fileInfo);
                        ms.Write(packegeBytes, 0, packegeBytes.Length);
                        allBytes = packegeBytes.Length;
                        byte[] fileBytes = null;
                        if (fullPackageLength > 0)
                            fileBytes = Utils.UnPacking(GetBytes(allBytes, ns, ms, fullPackageLength,imageClient));
                        ImageControler.ImageDeCompressor.ChangeCurrentPictureNew(data, fileBytes, blocks, widthResolution, heightResolution);
                    }
                    RdsControl.SetMainImageNewPic.Dispatcher.Invoke(
                        new Action(() =>   {
                                            RdsControl.SetMainImageNewPic.Source = ImageControler.ImageDeCompressor.CreateNewImage(data);
                                           }
                                           ));
                    ns.Write(answerBts, 0, answerBts.Length);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Unable to read data from the transport connection: An existing connection was forcibly closed by the remote host.")
                {
                    RdsControl.SetMainImageNewPic.Dispatcher.Invoke(new Action(() =>
                    {
                        RdsControl.MainWind.DeletePicture();
                        RdsControl.MainWind.CloseOutside();
                    }));
                    Console.WriteLine(ex.Message);
                    StopViewControl();
                }
                //MessageBox.Show(ex.Message);
                //if (responseThread != null)
                //    this.responseThread.Abort();
            }
        }

        public void StopViewControl()
        {
            try
            {
                this.controlTask.Abort();
                this.controlTask = new Thread(RunControl);
                this.controlTask.IsBackground = true;
                imageTask.Abort();
                imageTask = new Thread(RunImages);
                imageTask.IsBackground = true;
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
        }

    }
}
