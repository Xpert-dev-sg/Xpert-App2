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
        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            MainFrame.Navigate(new MenuPage());//MenuPage//TestControl
            TimeUtility.Initialize(MainFrame);
            TimeUtility.SystemServiceTimer();
 
            log.Info("System is initialized.");

            DBUtility.InitializeSystem();
            Door door = new Door();
            DoorMonitor monitor = new DoorMonitor(door);
            
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
    }
}