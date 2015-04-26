using RdsClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkWithImage;

namespace RdsClientControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class RdsControl : UserControl
    {
        Thread screenShotThread;
        Thread Thread;
        DoubleAnimation heightB_Animation;
        DoubleAnimation heightE_Animation;
        ThicknessAnimation MarginE_Animation;
        ThicknessAnimation MarginB_Animation;
        object lockObjeck = new object();

        MyWindowState lastState;

        Dictionary<string, RemoteServer> servers;
        string message;
        RdsReceiver rdsService;
        RemoteServer currentServer;

        bool isShow = false;
        bool isFullScreen = false;

        bool isControlModeOn = false;

        ViewMode viewMode;
        public static RdsControl MainWind { set; get; }

        public static BitmapSource Source { set; get; }


        public static Image SetMainImageNewPic
        {
            set;
            get;
        }

        Window mainWindow;


        public static WrapPanel ImageContainer { private set; get; }
        public RdsControl(Window window)
        {
            InitializeComponent();
            viewMode = ViewMode.enScaleMode;

            mainWindow = window;

            lastState = new MyWindowState();

            SetMainImageNewPic = this.ViewImage;
            //----------------------Height Animation Maximize
            heightB_Animation = new DoubleAnimation();
            heightB_Animation.From = 0;
            heightB_Animation.To = 20;
            heightB_Animation.FillBehavior = FillBehavior.HoldEnd;
            heightB_Animation.Duration = new Duration(TimeSpan.FromMilliseconds(100));

            //----------------------Height Animation Minimize
            heightE_Animation = new DoubleAnimation();
            heightE_Animation.From = 20;
            heightE_Animation.To = 0;
            heightE_Animation.FillBehavior = FillBehavior.HoldEnd;
            heightE_Animation.Duration = new Duration(TimeSpan.FromMilliseconds(100));

            //----------------------Margin Animation Down
            MarginB_Animation = new ThicknessAnimation();
            MarginB_Animation.From = new Thickness(0, 0, 0, 0);
            MarginB_Animation.To = new Thickness(0, 20, 0, 0);
            MarginB_Animation.FillBehavior = FillBehavior.HoldEnd;
            MarginB_Animation.AutoReverse = false;
            MarginB_Animation.Duration = new Duration(TimeSpan.FromMilliseconds(100));

            //----------------------Margin Animation UP

            MarginE_Animation = new ThicknessAnimation();
            MarginE_Animation.From = new Thickness(0, 20, 0, 0);
            MarginE_Animation.To = new Thickness(0, 0, 0, 0);
            MarginE_Animation.FillBehavior = FillBehavior.Stop;
            MarginE_Animation.AutoReverse = false;
            MarginE_Animation.Duration = new Duration(TimeSpan.FromMilliseconds(100));

            servers = new Dictionary<string, RemoteServer>();

            rdsService = new RdsReceiver(servers);
            rdsService.StartListen();
            MainWind = this;
            ImageContainer = this.ImgContainer;
            IntPtr hBmp = Images.close_window.GetHbitmap();
            this.Close.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            Utils.DeleteObject(hBmp);
            IntPtr hBmp_max = Images.maximize_window.GetHbitmap();
            this.Maximize.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp_max, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            Utils.DeleteObject(hBmp_max);
            IntPtr hBmp_keyboardC = Images.control_panel_1.GetHbitmap();
            this.KeyBoardControl.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp_keyboardC, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            Utils.DeleteObject(hBmp_keyboardC);
            IntPtr hBmp_arrow = Images._1427375812_double_arrow_down_32.GetHbitmap();
            this.Arrow.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp_arrow, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            Utils.DeleteObject(hBmp_arrow);
            IntPtr hBmp_screenMode = Images.display_mac_inv.GetHbitmap();
            this.ScreenMode.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp_screenMode, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            Utils.DeleteObject(hBmp_screenMode);
        }

        public void RebootClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var key = (sender as Image).Tag.ToString();
                this.currentServer = this.servers[key];
                this.currentServer.ShutDownOrReboot("REBOOT");
                DeletePicture();
                this.currentServer = null;
            }
            catch { }
        }


        public void ShutDownClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var key = (sender as Image).Tag.ToString();
                this.currentServer = this.servers[key];
                this.currentServer.ShutDownOrReboot("SHUTDOWN");
                DeletePicture();
                this.currentServer = null;
            }
            catch { }
        }


        public void LogoutClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var key = (sender as Image).Tag.ToString();
                this.currentServer = this.servers[key];
                this.currentServer.ShutDownOrReboot("LOGOFF");
                DeletePicture();
                this.currentServer = null;
            }
            catch { }
        }

        public void ImageClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var key = (sender as Image).Tag.ToString();
                this.ImgContainer.Visibility = Visibility.Hidden;
                this.PanelScroll.Visibility = System.Windows.Visibility.Hidden;
                this.PanelScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                this.PanelScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                this.PanelScroll.PanningMode = PanningMode.None;

                currentServer = this.servers[key];
                currentServer.StartView();
                this.ScrollViewer.Visibility = Visibility.Visible;
                this.ShowPanel.Visibility = Visibility.Visible;

                this.front_canvas.Visibility = System.Windows.Visibility.Visible;
                this.RdsIp.Text = String.Format("Ip address:[{0}]", currentServer.ipAddress);
            }
            catch
            {

            }
        }


        private void ViewImage_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.isControlModeOn)
                {
                    if (currentServer.data != null && ViewImage.Source != null)
                    {
                        //if (viewMode == ViewMode.enScaleMode)
                        //{
                        System.Windows.Point p = Mouse.GetPosition(ViewImage);
                        System.Windows.Point unscaled_p = new System.Windows.Point();

                        int w_i = currentServer.data.widthResolution;
                        int h_i = currentServer.data.heightResolution;
                        int w_c = (int)ViewImage.ActualWidth;
                        int h_c = (int)ViewImage.ActualHeight;


                        float imageRatio = w_i / (float)h_i;
                        float containerRatio = w_c / (float)h_c;

                        if (imageRatio >= containerRatio)
                        {
                            float scaleFactor = w_c / (float)w_i;
                            float scaledHeight = h_i * scaleFactor;
                            float filler = Math.Abs(h_c - scaledHeight) / 2;
                            unscaled_p.X = (int)(p.X / scaleFactor);
                            unscaled_p.Y = (int)((p.Y - filler) / scaleFactor);
                        }
                        else
                        {
                            float scaleFactor = h_c / (float)h_i;
                            float scaledWidth = w_i * scaleFactor;
                            float filler = Math.Abs(w_c - scaledWidth) / 2;
                            unscaled_p.X = (int)((p.X - filler) / scaleFactor);
                            unscaled_p.Y = (int)(p.Y / scaleFactor);
                        }
                        lock (lockObjeck)
                        {
                            this.message = String.Format("Move|{0},{1}", unscaled_p.X, unscaled_p.Y);
                            this.currentServer.StartControl(message);
                        }
                        //}
                        //else
                        //{
                        //    System.Windows.Point unscaled_p = Mouse.GetPosition(ViewImage);
                        //    lock (lockObjeck)
                        //    {
                        //        this.message = String.Format("Move|{0},{1}", unscaled_p.X, unscaled_p.Y);
                        //        this.currentServer.StartControl(message);
                        //    }
                        //}
                        this.ViewImage.Focus();
                    }
                }
            }
            catch { }
        }

        private void ViewImage_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (this.isControlModeOn)
                {
                    if (currentServer.data != null)
                    {
                        lock (lockObjeck)
                        {
                            var formsKey = (System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(e.Key);
                            this.message = String.Format("KEYDOWN|{0}", formsKey);
                            this.currentServer.StartControl(message);
                            Console.WriteLine("Key:[{0}]", e.Key);
                        }
                    }
                }
            }
            catch { }
        }

        private void ViewImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (this.isControlModeOn)
                {
                    if (currentServer.data != null)
                    {
                        lock (lockObjeck)
                        {
                            if (e.ChangedButton == MouseButton.Left)
                                this.message = String.Format("LUP|{0},{1}", 0, 0);
                            else if (e.ChangedButton == MouseButton.Right)
                                this.message = String.Format("RUP|{0},{1}", 0, 0);

                            this.currentServer.StartControl(message);

                        }
                    }
                }
            }
            catch { }
        }

        private void ViewImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (this.isControlModeOn)
                {
                    if (currentServer.data != null)
                    {
                        lock (lockObjeck)
                        {
                            if (e.ChangedButton == MouseButton.Left)
                                this.message = String.Format("LDOWN|{0},{1}", 0, 0);
                            else if (e.ChangedButton == MouseButton.Right)
                                this.message = String.Format("RDOWN|{0},{1}", 0, 0);
                            this.currentServer.StartControl(message);
                        }
                    }
                }
            }
            catch { }
        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isFullScreen)
                {
                    lastState.lastState = mainWindow.WindowState;
                    lastState.Top = mainWindow.Top;
                    lastState.Left = mainWindow.Left;
                    mainWindow.WindowStyle = WindowStyle.None;
                    mainWindow.WindowState = WindowState.Maximized;
                    mainWindow.Topmost = true;
                    mainWindow.Top = 0;
                    mainWindow.Left = 0;
                    //this.MyMenu.Visibility = Visibility.Hidden;
                    //this.MyMenu.Height = 0;
                    OneToOne_Click(null, null);
                }
                else
                {
                    mainWindow.WindowState = this.lastState.lastState;
                    mainWindow.WindowStyle = WindowStyle.ThreeDBorderWindow;
                    //this.MyMenu.Visibility = Visibility.Visible;
                    mainWindow.Left = lastState.Left;
                    mainWindow.Top = lastState.Top;
                    //this.MyMenu.Height = 18;

                }
                isFullScreen = !isFullScreen;
            }
            catch { }

        }

        private void ViewImage_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (this.isControlModeOn)
                {
                    if (currentServer.data != null)
                    {
                        lock (lockObjeck)
                        {
                            var formsKey = (System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(e.Key);
                            this.message = String.Format("KEYUP|{0}", formsKey);
                            this.currentServer.StartControl(message);
                            Console.WriteLine("Key:[{0}]", e.Key);
                        }
                    }
                }
            }
            catch { }
        }

        private void front_canvas_MouseEnter(object sender, MouseEventArgs e)
        {

            KeyboardIconChange();
            this.Maximize.Visibility = System.Windows.Visibility.Visible;
            this.Close.Visibility = System.Windows.Visibility.Visible;
            this.KeyBoardControl.Visibility = System.Windows.Visibility.Visible;
            this.front_canvas.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightB_Animation);
            KeyBoardControl.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightB_Animation);
            Maximize.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightB_Animation);
            Close.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightB_Animation);
            RdsIp.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightB_Animation);
        }

        private void front_canvas_MouseLeave(object sender, MouseEventArgs e)
        {

            this.front_canvas.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightE_Animation);
            Maximize.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightE_Animation);
            Close.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightE_Animation);
            RdsIp.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightE_Animation);
            KeyBoardControl.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightE_Animation);
            this.Maximize.Visibility = System.Windows.Visibility.Hidden;
            this.Close.Visibility = System.Windows.Visibility.Hidden;
            this.KeyBoardControl.Visibility = System.Windows.Visibility.Hidden;
        }

        public void CloseOutside()
        {
            Close_MouseDown(null, null);
        }


        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
                if(currentServer!=null)
                    currentServer.StopViewControl();
                this.ViewImage.Source = null;
                this.ScrollViewer.Visibility = Visibility.Hidden;
                this.front_canvas.Visibility = System.Windows.Visibility.Hidden;
                this.ImgContainer.Visibility = System.Windows.Visibility.Visible;
                this.ShowPanel.Visibility = System.Windows.Visibility.Hidden;
                this.PanelScroll.Visibility = System.Windows.Visibility.Visible;
                this.PanelScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                if (this.isControlModeOn)
                {
                    KeyboardIconChange();
                    this.isControlModeOn = false;
                }
                if (isFullScreen)
                    FullScreen_Click(null, null);
        }

        private void Maximize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FullScreen_Click(null, null);
        }

        private void KeyBoardControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyboardIconChange();
            this.isControlModeOn = !this.isControlModeOn;

        }
        private void KeyboardIconChange()
        {
            IntPtr hBmp_keyboard = this.isControlModeOn ? Images.control_panel_1.GetHbitmap() : Images.control_panel_2.GetHbitmap();
            this.KeyBoardControl.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp_keyboard, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            Utils.DeleteObject(hBmp_keyboard);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isShow)
            {
                ShowPanel.BeginAnimation(Grid.MarginProperty, MarginB_Animation);
                IntPtr hBmp_arrow = Images.arrowUp.GetHbitmap();
                this.Arrow.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp_arrow, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                Utils.DeleteObject(hBmp_arrow);
                this.Maximize.Visibility = System.Windows.Visibility.Visible;
                this.Close.Visibility = System.Windows.Visibility.Visible;
                this.KeyBoardControl.Visibility = System.Windows.Visibility.Visible;
                this.ScreenMode.Visibility = System.Windows.Visibility.Visible;
                this.front_canvas.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightB_Animation);
                KeyBoardControl.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightB_Animation);
                Maximize.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightB_Animation);
                Close.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightB_Animation);
                RdsIp.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightB_Animation);
                ScreenMode.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightB_Animation);
            }
            else
            {
                ShowPanel.BeginAnimation(Grid.MarginProperty, MarginE_Animation);
                IntPtr hBmp_arrow = Images._1427375812_double_arrow_down_32.GetHbitmap();
                this.Arrow.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp_arrow, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                Utils.DeleteObject(hBmp_arrow);
                this.front_canvas.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightE_Animation);

                ScreenMode.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightE_Animation);
                Maximize.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightE_Animation);
                Close.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightE_Animation);
                RdsIp.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightE_Animation);
                KeyBoardControl.BeginAnimation(System.Windows.Shapes.Rectangle.HeightProperty, heightE_Animation);
                this.Maximize.Visibility = System.Windows.Visibility.Hidden;
                this.Close.Visibility = System.Windows.Visibility.Hidden;
                this.KeyBoardControl.Visibility = System.Windows.Visibility.Hidden;
                this.ScreenMode.Visibility = System.Windows.Visibility.Hidden;
            }
            isShow = !isShow;
        }

        private void OneToOne_Click(object sender, RoutedEventArgs e)
        {
            //this.Scale.Icon = null;
            this.ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            this.ScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            this.ViewImage.Stretch = Stretch.Uniform;
            viewMode = ViewMode.enOneToOne;
        }
        private void Scale_Click(object sender, RoutedEventArgs e)
        {
            //this.OneToOne.Icon = null ;
            this.ViewImage.Stretch = Stretch.Uniform;
            this.ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            this.ScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            this.ScrollViewer.PanningMode = PanningMode.None;
            viewMode = ViewMode.enScaleMode;


        }

        public void DeletePicture()
        {
            try
            {
                Grid currentGrid = null;
                foreach (var child in ImageContainer.Children)
                {
                    var tag = (child as Grid).Tag.ToString();
                    if (tag == currentServer.ipAddress)
                    {
                        currentGrid = (child as Grid);
                    }
                }

                if (currentGrid != null)
                {
                    servers.Remove(currentGrid.Tag.ToString());
                    ImageContainer.Children.Remove(currentGrid);
                }
            }
            catch { }

        }

        private void ScreenMode_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.viewMode == ViewMode.enScaleMode)
            {
                OneToOne_Click(null, null);
                this.ScreenMode_T1.Text = "One to One Mode On";
                this.ScreenMode_T2.Text = "Click to enable Scale Mode";
            }
            else
            {
                Scale_Click(null, null);
                this.ScreenMode_T1.Text = "Scale Mode On";
                this.ScreenMode_T2.Text = "Click to enable One to One Mode";
            }
        }

    }
}
