using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class AccessPage : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ObservableCollection<string> items;
        private DispatcherTimer timer;

        private int _countdown = 0;
        private DispatcherTimer _timer;
        public AccessPage()
        {
            InitializeComponent();

            //MonitorKeyMouseUntility.MonitorKeyMouseMain();

            Setup_timer();//设置定时器 回主页



            // 设置定时器
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // 每一秒触发一次
            timer.Tick += Timer_Tick;
            timer.Start();


            Load_dataComponents();
        }

        private void Setup_timer()
        {
            //TimeUtility.CarouselMenuTimer();
            //

            _countdown = DB_Base.SystemMenuInterval_admin;
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
                try
                {
                    NavigationService.Navigate(new MenuPage());
                }
                catch (Exception)
                {
                }

            }
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            EventDB eventDB = new EventDB();
            //return_list.ItemsSource = DB_Base.returnlist;
            //borrow_list.ItemsSource = DB_Base.borrowlist;
            borrowdataGrid.ItemsSource = eventDB.GetBorrowRecords_user_access(DB_Base.CurrentUser.UserName);
            //// 更新 ListView 的数据，这里可以是任何你需要的更新逻辑
            //items.Add(DateTime.Now.ToString("HH:mm:ss"));

            //// 为了防止列表无限增长，移除最早的项
            //if (items.Count > 10)
            //{
            //    items.RemoveAt(0);
            //}
        }

        public void Load_dataComponents()
        {
            txtDepartment.Text = DB_Base.CurrentUser.DepartmentId;
            txtEmail.Text = DB_Base.CurrentUser.Email;
            txtusername.Text = DB_Base.CurrentUser.UserName;

            EventDB eventDB = new EventDB();
            borrowdataGrid.ItemsSource = eventDB.GetBorrowRecords_user_access(DB_Base.CurrentUser.UserName);

            //return_list.ItemsSource = DB_Base.returnlist;
            //borrow_list.ItemsSource = DB_Base.borrowlist;
        }

        //private void OpenDoorButton_Click(object sender, RoutedEventArgs e)
        //{
        //    DB_Base.RFIDList_o = RFIDUtility.Read_RFID();
        //    DB_Base.borrowUser = DB_Base.CurrentUser;

        //    DoorUtility.OpenDoor();


        //}

        private void OpenRightDoorButton_Click(object sender, RoutedEventArgs e)
        {
            DB_Base.RFIDList_o = RFIDUtility.Read_RFID();
            DB_Base.borrowUser = DB_Base.CurrentUser;
            DoorUtility.OpenRightDoor();
        }

        private void OpenAllDoorButton_Click(object sender, RoutedEventArgs e)
        {
            DB_Base.RFIDList_o = RFIDUtility.Read_RFID();
            DB_Base.borrowUser = DB_Base.CurrentUser;
            DoorUtility.OpenAllDoor();
        }

        private void OpenLeftDoorButton_Click(object sender, RoutedEventArgs e)
        {
            //DB_Base.RFIDList_o = RFIDUtility.Read_RFID();
            //DB_Base.borrowUser = DB_Base.CurrentUser;
            //DoorUtility.OpenLeftDoor();

            /////t1
            //ContentDB contentDB = new ContentDB();
            //contentDB.StoreReset("0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|;0000000000000000100010|");

            /////t3
            //DB_Base.RFIDList_o = "0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|;0000000000000000100010|";
            //DB_Base.RFIDList_c = "0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|";

            /////t5
            //DB_Base.RFIDList_o = "0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|";
            //DB_Base.RFIDList_c = "0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|;0000000000000000100010|";

            /////t7
            //DB_Base.RFIDList_o = "0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|;0000000000000000100010|";
            //DB_Base.RFIDList_c = "0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|;0000000000000000100011|";
            //0000000000000000100015 |
            //0000000000000000100015 |

            /////t9
            //ContentDB contentDB = new ContentDB();
            //contentDB.StoreReset(@"0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|;0000000000000000100010|;" +
            //                   "0000000000000000100015|;0000000000000000100017|;0000000000000000100018|;0000000000000000100019|;0000000000000000100020|;" +
            //                   "0000000000000000100025|;0000000000000000100027|;0000000000000000100028|;0000000000000000100029|;0000000000000000100030|;" +
            //                   "0000000000000000100035|;0000000000000000100037|;0000000000000000100038|;0000000000000000100039|;0000000000000000100040|;" +
            //                   "0000000000000000100055|;0000000000000000100057|;0000000000000000100058|;0000000000000000100059|;0000000000000000100060|;" 
             //                   );

            /////t11
            //DB_Base.RFIDList_o = @"0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|;0000000000000000100010|;" +
            //                   "0000000000000000100015|;0000000000000000100017|;0000000000000000100018|;0000000000000000100019|;0000000000000000100020|;" +
            //                   "0000000000000000100025|;0000000000000000100027|;0000000000000000100028|;0000000000000000100029|;0000000000000000100030|;" +
            //                   "0000000000000000100035|;0000000000000000100037|;0000000000000000100038|;0000000000000000100039|;0000000000000000100040|;" +
            //                   "0000000000000000100055|;0000000000000000100057|;0000000000000000100058|;0000000000000000100059|;0000000000000000100060|;";

            //DB_Base.RFIDList_c = @"0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|;0000000000000000100010|;" +
            //                   "0000000000000000100015|;0000000000000000100017|;0000000000000000100018|;0000000000000000100019|;0000000000000000100020|;" +
            //                   "0000000000000000100025|;0000000000000000100027|;0000000000000000100028|;0000000000000000100029|;0000000000000000100030|;" +
            //                   "0000000000000000100035|;0000000000000000100037|;0000000000000000100038|;0000000000000000100039|;0000000000000000100040|;";

            /////t13
            //DB_Base.RFIDList_o = @"0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|;0000000000000000100010|;" +
            //                   "0000000000000000100015|;0000000000000000100017|;0000000000000000100018|;0000000000000000100019|;0000000000000000100020|;" +
            //                   "0000000000000000100025|;0000000000000000100027|;0000000000000000100028|;0000000000000000100029|;0000000000000000100030|;" +
            //                   "0000000000000000100035|;0000000000000000100037|;0000000000000000100038|;0000000000000000100039|;0000000000000000100040|;";

            //DB_Base.RFIDList_c = @"0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|;0000000000000000100010|;" +
            //                   "0000000000000000100015|;0000000000000000100017|;0000000000000000100018|;0000000000000000100019|;0000000000000000100020|;" +
            //                   "0000000000000000100025|;0000000000000000100027|;0000000000000000100028|;0000000000000000100029|;0000000000000000100030|;" +
            //                   "0000000000000000100035|;0000000000000000100037|;0000000000000000100038|;0000000000000000100039|;0000000000000000100040|;" +
            //                   "0000000000000000100055|;0000000000000000100057|;0000000000000000100058|;0000000000000000100059|;0000000000000000100060|;";

            /////t15
            //DB_Base.RFIDList_o = "0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|;0000000000000000100010|";
            //DB_Base.RFIDList_c = "0000000000000000100005|;0000000000000000100007|;0000000000000000100008|;0000000000000000100009|;0000000000000000100011|";

            DB_Base.borrowUser = DB_Base.CurrentUser;
            NotePopup notePopup = new NotePopup();
            notePopup.Show();
        }
    }
}
