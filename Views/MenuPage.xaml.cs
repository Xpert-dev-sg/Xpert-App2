﻿using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XpertApp2.DB;

namespace XpertApp2.Views
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MenuPage()
        {
            InitializeComponent();
        }

        private void Click_ContentList(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new ContentListViewPage());
        }

        private void Click_Access(object sender, MouseButtonEventArgs e)
        {
            DB_Base.currentpage= "Access";
            loginPopup loginPage = new loginPopup();
            loginPage.ShowDialog();
            //if(DB_Base.Islogined)
            //{
            //    this.NavigationService.Navigate(new AccessPage());
            //}
        }

        private void Click_Admin(object sender, MouseButtonEventArgs e)
        {
            DB_Base.currentpage = "Admin";
            loginPopup loginPage = new loginPopup();
            loginPage.ShowDialog();
            //if (DB_Base.Islogined)
            //{
            //    this.NavigationService.Navigate(new AdminPage());
            //}
        }
    }
}
