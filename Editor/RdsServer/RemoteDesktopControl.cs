using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RdsServer
{
    public class RemoteDesktopControl
    {


        internal const int SE_PRIVILEGE_ENABLED = 0x00000002;
        internal const int TOKEN_QUERY = 0x00000008;
        internal const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        internal const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        static extern bool keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);


        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);



       // [StructLayout(LayoutKind.Sequential, Pack = 1)]

        public static void MouseUp(Point Cursor, MouseEvent mEvent)
        {
            mouse_event((uint)mEvent, (uint)Cursor.X, (uint)Cursor.Y, (uint)0, (UIntPtr)0);
        }

        public static void MouseDown(Point Cursor, MouseEvent mEvent)
        {
            mouse_event((uint)mEvent, (uint)Cursor.X, (uint)Cursor.Y, (uint)0, (UIntPtr)0);
        }

        public static void MouseMove(Point Cursor)
        {
            SetCursorPos((int)Cursor.X, (int)Cursor.Y);
        }

        public static void KeyboardEventDown(int keyCode)
        {
            keybd_event((byte)keyCode, 0, 0x0001 | 0, (UIntPtr)0);
        }
        public static void KeyboardEventUp(int keyCode)
        {
            keybd_event((byte)keyCode, 0, 0x0001 | 0x0002, (UIntPtr)0);
        }


        public static Point GetCoordsToCLick(string msg)
        {
            Point pt = new Point();
            var data = msg.Split(',');
            pt.X = Convert.ToInt32(data[0]);
            pt.Y = Convert.ToInt32(data[1]);
            return pt;
        }

        public static int GetKeyCode(string code)
        {
            System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)Enum.Parse(typeof(System.Windows.Forms.Keys), code, true);
            return (int)key;
        }



        [DllImport("user32.dll")]
        public static extern void LockWorkStation();
        [DllImport("user32.dll")]
        public static extern int ExitWindowsEx(int uFlags, int dwReason);



        public static void WindowsManagment(WindowsManagmentKeys key)
        {
            if(key==WindowsManagmentKeys.enReboot)
            {
                //var regisrtyKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\", true);
                //regisrtyKey.SetValue("RdsServer.exe", Application.ExecutablePath);
                System.Diagnostics.Process.Start("shutdown", "/r /t 5");
            }
            else if(key == WindowsManagmentKeys.enShutDown)
            {
                System.Diagnostics.Process.Start("shutdown", "/s /t 5");
            }
            ExitWindowsEx((int)key, 0);
        }


        /*
        // Lock workstation
        public static void BtnLockCompClick()
        {
            LockWorkStation();
        }

        // Log Off
        public static void BtnLogOffClick()
        {
            //if (DialogResult.Yes == MessageBox.Show("Do you really want to Log Off?", "Log Off", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                ExitWindowsEx(0, 0);
        }

        // Reboot
        public static void Reboot()
        {
            //if (DialogResult.Yes == MessageBox.Show("Do you really want to Reboot?", "Reboot", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                ExitWindowsEx(2, 0);
        }

        // Shutdown
        public static void ShutDown()
        {
            //if (DialogResult.Yes == MessageBox.Show("Do you really want to Shutdown?", "Shutdown", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                ExitWindowsEx(1, 0);
        }

        // Force LogOff
        public static void ForceLogOff()
        {
            //if (DialogResult.Yes == MessageBox.Show("Do you really want to force Log Off?", "Force LogOff", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                ExitWindowsEx(4, 0);
        }

        // Hibernate
        public static void BtnHibernateClick()
        {
            System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Hibernate, true, true);
        }
         * */
    }

        public enum MouseEvent
    {
        MOUSEEVENTF_ABSOLUTE = 0x8000,
        MOUSEEVENTF_LEFTDOWN = 0x0002,
        MOUSEEVENTF_LEFTUP = 0x0004,
        MOUSEEVENTF_MIDDLEDOWN = 0x0020,
        MOUSEEVENTF_MIDDLEUP = 0x0040,
        MOUSEEVENTF_MOVE = 0x0001,
        MOUSEEVENTF_RIGHTDOWN = 0x0008,
        MOUSEEVENTF_RIGHTUP = 0x0010,
        MOUSEEVENTF_XDOWN = 0x0080,
        MOUSEEVENTF_XUP = 0x0100,
        MOUSEEVENTF_WHEEL = 0x0800,
        MOUSEEVENTF_HWHEEL = 0x01000
    }

    public enum WindowsManagmentKeys
    {
        enLogOff = 0,
        enShutDown = 1,
        enReboot = 2,
        enForceLogOff = 4
    }



}
