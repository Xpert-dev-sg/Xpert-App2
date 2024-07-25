using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Input;
using XpertApp2.DB;
using XpertApp2.Views;

namespace XpertApp2.Utility
{
    public static  class MonitorKeyMouseUntility
    {

        
        // 钩子句柄
        private static IntPtr _mouseHookID = IntPtr.Zero;
        private static IntPtr _keyboardHookID = IntPtr.Zero;
        private static System.Timers.Timer _timer;
        private static DateTime _lastEventTime;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void MonitorKeyMouseMain()
        {


            _lastEventTime = DateTime.Now;

            LowLevelMouseProc mouseProc = MouseHookCallback;
            LowLevelKeyboardProc keyboardProc = KeyboardHookCallback;
            _mouseHookID = SetMouseHook(mouseProc);
            _keyboardHookID = SetKeyboardHook(keyboardProc);

            _timer = new System.Timers.Timer(1000); // Check every second
            _timer.Elapsed += CheckMouseActivity;
            _timer.Start();

            //Application.Run();

            // 程序结束时取消钩子
            UnhookWindowsHookEx(_mouseHookID);
            UnhookWindowsHookEx(_keyboardHookID);
        }

        private static void CheckMouseActivity(object sender, ElapsedEventArgs e)
        {
            if ((DateTime.Now - _lastEventTime).TotalMinutes >= 1)
            {
                UserBD udb= new UserBD();
                udb.LogoutUser();
                
                //MoveMouse();
                _lastEventTime = DateTime.Now;
            }
        }

        //private static void MoveMouse()
        //{
        //    // Move the mouse by a small amount to simulate activity
        //    int x = Cursor.Position.X + 1;
        //    int y = Cursor.Position.Y + 1;
        //    //SetCursorPos(x, y);

        //    LeftClick(x, y);
        //}

        // 设置钩子
        private static IntPtr SetMouseHook(LowLevelMouseProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr SetKeyboardHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        // 低级钩子委托
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        // 钩子回调函数
        private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                _lastEventTime = DateTime.Now;
                //Console.WriteLine($"Mouse event detected at: {_lastEventTime}");
            }
            return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);
        }

        private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                _lastEventTime = DateTime.Now;
                //Console.WriteLine($"Keyboard event detected at: {_lastEventTime}");
            }
            return CallNextHookEx(_keyboardHookID, nCode, wParam, lParam);
        }

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;
        public const int MOUSEEVENTF_WHEEL = 0x0800;

        public static void MoveMouse(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void LeftClick(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)x, (uint)y, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, UIntPtr.Zero);
        }


        // 声明钩子类型
        private const int WH_MOUSE_LL = 14;
        private const int WH_KEYBOARD_LL = 13;


        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

    }
}
