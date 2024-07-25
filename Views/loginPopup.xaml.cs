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
using System.Windows.Shapes;
using System.Windows.Threading;
using XpertApp2.DB;
using XpertApp2.Utility;

namespace XpertApp2.Views
{
    /// <summary>
    /// Interaction logic for loginPopup.xaml
    /// </summary>
    public partial class loginPopup : Window
    {
        public loginPopup()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(DB_Base.SystemMenuInterval);
            timer.Tick += CarouselTimer_Tick;
            timer.Start();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                HandleLogin();
            }
        }


        private void HandleLogin()
        {
            string password = loginbox.Password;

            UserBD udb = new UserBD();
            var isPass = udb.IsLogined(password);
            if (isPass)
            {
                DB_Base.Islogined = true;
                
                this.Hide();
              
                switch (DB_Base.currentpage)
                {
                    case "Access":
                        TimeUtility.navigationAccess();
                        break;
                    case "Admin":
                        TimeUtility.navigationAdmin();
                        break;
                }
            }
            else
            {
                DB_Base.Islogined = false;
                MessageBox.Show("wrong pass try again");
            }

        }

        private  void CarouselTimer_Tick(object sender, EventArgs e)
        {
            
            this.Close();

        }
    }
}
