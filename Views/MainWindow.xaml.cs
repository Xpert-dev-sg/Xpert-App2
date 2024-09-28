using System.Configuration;
using System.IO.Ports;
using System.Net.NetworkInformation;
using System.Text;
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

namespace XpertApp2.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private StringBuilder cardData;
        public MainWindow()
        {

            InitializeComponent();

            //testc();
            InitializeTimer();
            MainFrame.Navigate(new MenuPage());//MenuPage//TestControl
            TimeUtility.Initialize(MainFrame);
            TimeUtility.SystemServiceTimer();

            log.Info("System is initialized.");

            DBUtility.InitializeSystem();
            //EmailUtility.SendEmail("test", "test", new string[] { "wangyiwater77@163.com" });
            //EmailUtility.SendEmail1("test", "test", new string[] { "wangyiwater77@163.com" });
            //EmailUtility.SendEmail2("test", "test", new string[] { "wangyiwater77@163.com" });
            Door door = new Door();
            DoorMonitor monitor = new DoorMonitor(door);
            DoorUtility doorUtility = new DoorUtility();

            // 窗口加载时开始监听键盘输入 (假设读卡器模拟键盘输入)
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        private void testc()
        {
            var rfid1 = "1;2;3";//before open
            var rfid2 = "1;2;5";//after close
            string[] array1 = rfid1.Split(';');
            string[] array2 = rfid2.Split(';');

            // 找出string1中有但string2中没有的元素
            var borrow_arr = array1.Except(array2);

            // 找出string2中有但string1中没有的元素
            var return_arr = array2.Except(array1);

            MessageBox.Show("borrow_arr:" + string.Join(";", borrow_arr) + " return_arr:" + string.Join(";", return_arr));  
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // 设置定时间隔为1秒
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTimeTextBlock.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            UpdateNetworkStatus();
        }

        private void UpdateNetworkStatus()
        {
            bool isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();
            if (isNetworkAvailable)
            {
                wifi_img.Source = new BitmapImage(new Uri("pack://application:,,,/XpertApp2;component/Resources/Img/wifi.jpg"));
            }
            else
            {
                wifi_img.Source = new BitmapImage(new Uri("pack://application:,,,/XpertApp2;component/Resources/Img/no_wifi.jpg"));
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var islogined = !(DB_Base.CurrentUser == null);
            if (islogined)
            {
                UserDB udb = new UserDB();
                udb.LogoutUser();
            }
            MainFrame.NavigationService.Navigate(new MenuPage());
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // 如果检测到 Enter 键，表示卡片数据输入完毕
                if (e.Key == Key.Enter)
                {
                    string cardString = cardData.ToString();
                    if (!string.IsNullOrEmpty(cardString))
                    {


                        log.Info("read card log in");
                        //MessageBox.Show(cardString);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            HandleLogin(cardString);
                        });



                        // 清空 StringBuilder 以便接收下一张卡
                        cardData.Clear();
                    }
                    e.Handled = true;  // 阻止 Enter 事件继续传播
                }
                else
                {
                    // 获取按下的字符并追加到 cardData 中
                    if (e.Key >= Key.D0 && e.Key <= Key.Z || e.Key == Key.Space)
                    {
                        // 获取字符并添加到 StringBuilder
                        string? inputChar = new KeyConverter().ConvertToString(e.Key);
                        if (!string.IsNullOrEmpty(inputChar))
                        {
                            cardData.Append(inputChar);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error reading card data: " + ex.Message);
                System.Windows.MessageBox.Show("Error" + ex.Message);
            }

        }

        private static void HandleLogin(string pass)
        {
            try
            {
                string password = pass;

                UserDB udb = new UserDB();
                var isPass = udb.IsLogined(password);
                if (isPass)
                {
                    DB_Base.Islogined = true;

                    TimeUtility.navigationAccess();

                    DB_Base.RFIDList_o = RFIDUtility.Read_RFID();
                    DB_Base.borrowUser = DB_Base.CurrentUser;
                    DoorUtility.OpenAllDoor();

                }
                else
                {
                    loginPopup loginPopup = new loginPopup();
                    loginPopup.Show();
                    DB_Base.Islogined = false;
                    MessageBox.Show("wrong pass try again");

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //System.Windows.MessageBox.Show("Error" + ex.Message);
            }


        }


    }
}