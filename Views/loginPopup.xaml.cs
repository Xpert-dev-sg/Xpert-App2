using HandyControl.Controls;
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
using System.Windows.Threading;
using XpertApp2.DB;
using XpertApp2.Utility;
using MessageBox = System.Windows.MessageBox;
using Window = System.Windows.Window;

namespace XpertApp2.Views
{
    /// <summary>
    /// Interaction logic for loginPopup.xaml
    /// </summary>
    public partial class loginPopup : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         private int _countdown = 10;
        private DispatcherTimer _timer;
        public loginPopup()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            Setup_timer();
            loginbox.Focus();
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

            UserDB udb = new UserDB();
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
            loginbox.Clear();
        }


        private void Setup_timer()
        {
            //TimeUtility.CarouselMenuTimer();
            //
            var islogined = !(DB_Base.CurrentUser == null);
            _countdown = islogined ? DB_Base.SystemMenuInterval_admin : DB_Base.SystemMenuInterval;
            // 初始化计时器
            _timer = new DispatcherTimer();
            _timer.Interval = System.TimeSpan.FromSeconds(1);  // 每秒触发一次
            _timer.Tick += Timer_countdown_Tick;
            _timer.Start();

            // 设置初始显示数字
            txtcountdown.Text = _countdown.ToString();
        }
        private void Timer_countdown_Tick(object sender, System.EventArgs e)
        {
            _countdown--;

            // 更新显示的数字
            if (_countdown >= 0)
            {
                txtcountdown.Text = _countdown.ToString();
            }
            else
            {
                // 停止计时器并关闭窗口
                _timer.Stop();
                this.Close();
            }
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取被点击的按钮
            var button = sender as Button;
            if (button != null)
            {
                if (button.Content.ToString() == "-")
                {
                    if (loginbox.Password.Length > 0)
                    {
                        loginbox.Password = loginbox.Password.Substring(0, loginbox.Password.Length - 1);
                    }
                }
                else
                {
                    // 将按钮的内容（数字）添加到 PasswordBox
                    loginbox.Password += button.Content.ToString();
                }
                    
            }
        }

        // Enter 按钮点击事件处理
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            HandleLogin();  
            //string enteredPassword = loginbox.Password;

            //// 执行登录验证逻辑 (可以与后台服务对接)
            //if (enteredPassword == "1234")  // 这里可以替换成你需要验证的密码
            //{
            //    MessageBox.Show("Login Successful!");
            //}
            //else
            //{
            //    MessageBox.Show("Invalid Password!");
            //}

            // 清空 PasswordBox

        }

    }
}
