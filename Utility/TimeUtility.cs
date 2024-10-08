﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using XpertApp2.DB;
using XpertApp2.Views;

namespace XpertApp2.Utility
{
    public static class TimeUtility
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Frame _mainFrame;

        public static void Initialize(Frame frame)
        {
            _mainFrame = frame;
        }
        public static void CarouselMenuTimer()
        {
            var islogined = !(DB_Base.CurrentUser == null);
            var interval = islogined ? DB_Base.SystemMenuInterval_admin : DB_Base.SystemMenuInterval;


            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(interval);
            timer.Tick += CarouselTimer_Tick;
            timer.Start();
        }
        private static void CarouselTimer_Tick(object sender, EventArgs e)
        {
            // 回到主菜单
            _mainFrame.NavigationService.Navigate(new MenuPage());


            // 停止计时器
            DispatcherTimer timer = sender as DispatcherTimer;
            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= CarouselTimer_Tick;
            }
        }

        public static void UserLiveTimer()
        {

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(DB_Base.SystemMenuInterval_admin);
            timer.Tick += UserLive_Tick;
            timer.Start();
        }
        private static void UserLive_Tick(object sender, EventArgs e)
        {
            if (DB_Base.Islogined)
            {
                // 回到主菜单
                _mainFrame.NavigationService.Navigate(new MenuPage());
            }

            // 停止计时器
            DispatcherTimer timer = sender as DispatcherTimer;
            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= CarouselTimer_Tick;
            }
        }

        public static void SystemServiceTimer()
        {

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += SystemService_Tick;
            timer.Start();
        }
        private static void SystemService_Tick(object sender, EventArgs e)
        {
            var cm=Convert.ToInt32(DB_Base.systemservice_time.Split(':')[1]);
            var ch = Convert.ToInt32(DB_Base.systemservice_time.Split(':')[0]);
            var m=DateTime.Now.Minute;
            var h = DateTime.Now.Hour;
            if (ch==h && cm==m)
            {
               DBUtility.verifyBorrowTime();
            }

            // 停止计时器
            DispatcherTimer timer = sender as DispatcherTimer;
            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= SystemService_Tick;
            }
        }

        public static void navigationAccess()
        {
            _mainFrame.NavigationService.Navigate(new AccessPage());
        }

        public static void navigationAdmin()
        {
            _mainFrame.NavigationService.Navigate(new AdminPage());
        }

        public static void navigationMain()
        {
            _mainFrame.NavigationService.Navigate(new MenuPage());
        }

    }
}
