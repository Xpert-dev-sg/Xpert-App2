using HandyControl.Tools.Extension;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
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

            UserBD udb= new UserBD();
            var isPass= udb.IsLogined(password);
            if (isPass) 
            {
                DB_Base.Islogined = true;
                this.Hide();
            }
            else
            {
                DB_Base.Islogined = false;
                MessageBox.Show("wrong pass try again");
            }
            
        }
    }
}
