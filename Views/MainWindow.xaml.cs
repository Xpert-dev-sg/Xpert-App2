using System.Configuration;
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
        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            MainFrame.Navigate(new MenuPage());
            TimeUtility.Initialize(MainFrame);
 

            
            InitializeSystem();
            
        }
        private void InitializeSystem()
        {
            DB_Base.DBConnectionString = ConfigurationManager.AppSettings["DBConnectionString"];
            DB_Base.SystemMail = ConfigurationManager.AppSettings["SystemMail"];
            DB_Base.SystemMenuInterval = Convert.ToInt32( ConfigurationManager.AppSettings["SystemMenuInterval"]);
            DB_Base.SystemMenuInterval_admin = Convert.ToInt32(ConfigurationManager.AppSettings["SystemMenuInterval_admin"]);

            ContentDB contentDB = new ContentDB();
            contentDB.CreateContent();

            UserBD userBD = new UserBD();
            userBD.CreateUser();
            EventDB eventDB = new EventDB();
            eventDB.CreateEvent();
           
            eventDB.CreateBorrowRecords();
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
    }
}