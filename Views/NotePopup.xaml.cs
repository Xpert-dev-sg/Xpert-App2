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
    /// Interaction logic for NotePopup.xaml
    /// </summary>
    public partial class NotePopup : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int _countdown = 10;
        private DispatcherTimer _timer;
        //private string str = System.Reflection.MethodBase.GetCurrentMethod().Name;
        public NotePopup()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            Setup_timer();
            Load_dataComponents();
        }

        private void Setup_timer()
        {
            //TimeUtility.CarouselMenuTimer();
            //
            var islogined = !(DB_Base.CurrentUser == null);
            _countdown = DB_Base.SystemMenuInterval;
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

        public void Load_dataComponents()
        {
            Compare_RFID();
            txtusername.Text = DB_Base.borrowUser.UserName==null?"system": DB_Base.borrowUser.UserName;

            //return_list.ItemsSource = DB_Base.returnlist;
            //borrow_list.ItemsSource = DB_Base.borrowlist;
        }
        public void Compare_RFID()
        {
            try
            {
                string rfid2 = DB_Base.RFIDList_c;
                string rfid1 = DB_Base.RFIDList_o;

                log.Info($"after close:{string.Join(",", rfid1)}");
                log.Info($"before open:{string.Join(",", rfid2)}");
                EventDB eventDB = new EventDB();
                ContentDB contentDB = new ContentDB();

                // 将字符串拆分为数组，使用中文逗号“，”作为分隔符
                string[] array1 = rfid1.Split(';');
                string[] array2 = rfid2.Split(';');

                // 找出string1中有但string2中没有的元素
                var borrow_arr = array1.Except(array2);

                // 找出string2中有但string1中没有的元素
                var return_arr = array2.Except(array1);

                DB_Base.borrowlist =  new System.Collections.ObjectModel.ObservableCollection<Items>();
                DB_Base.returnlist =  new System.Collections.ObjectModel.ObservableCollection<Items>();

                string borrowlist_str = "";
                string returnlist_str = "";
                string[] tomail = new string[] { DB_Base.borrowUser.Email };
                var Subject = "";
                var msg = "";
                string b = "";
                string r = "";
                foreach (var item in borrow_arr)//borrow
                {
                    eventDB.InsertBorrowRecords(item, DB_Base.borrowUser.UserName);
                    contentDB.UpdateContent_on_hand(item, DB_Base.borrowUser.UserName);//update content on hand
                    Items items = new Items();
                    items.Item_Name = contentDB.GetContent_rfid(item);
                    DB_Base.borrowlist.Add(items);
                    //DBUtility.verifyBorrow(item);//verify if user is allowed to borrow this item
                    b += item + ",";
                }

                foreach (var item in return_arr)//return
                {
                    eventDB.UpdateBorrowRecords_return(item);//, DB_Base.borrowUser.UserName
                    contentDB.UpdateContent_on_hand(item, "");
                    Items items = new Items();
                    items.Item_Name = contentDB.GetContent_rfid(item);
                    DB_Base.returnlist.Add(items);
                    r += item + ",";
                }
                log.Info($"borrow:{string.Join(",", b)}");
                log.Info($"return:{string.Join(",", r)}");
                if (DB_Base.borrowlist.Count() > 0)
                {
                    Subject = " borrwo";
                    foreach (var item in DB_Base.borrowlist)
                    {
                        borrowlist_str += item.Item_Name + ",";
                    }
                    msg += $"{DB_Base.borrowUser.UserName} take {borrowlist_str} out on {DateTime.Now}.</br>";
                }

                if (DB_Base.returnlist.Count() > 0)
                {
                    Subject += " return";

                    foreach (var item in DB_Base.returnlist)
                    {
                        returnlist_str += item.Item_Name + ",";
                    }
                    msg += $"{DB_Base.borrowUser.UserName} return {returnlist_str} out on {DateTime.Now}.</br>";

                }
                log.Info($"borrow:{string.Join(",", borrowlist_str)}");
                log.Info($"return:{string.Join(",", returnlist_str)}");
                if (!string.IsNullOrEmpty(msg))
                {
                    //MessageBox.Show(msg, "Send by Email");
                    //EmailUtility.SendEmail(msg, Subject + "item", tomail);
                }
                return_list.ItemsSource = DB_Base.returnlist;
                borrow_list.ItemsSource = DB_Base.borrowlist;
            }
            catch (Exception ex)
            {

                log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Error");
            }

        }
    }
}
