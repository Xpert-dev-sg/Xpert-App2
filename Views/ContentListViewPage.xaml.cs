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
using XpertApp2.Models;
using XpertApp2.Utility;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace XpertApp2.Views
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class ContentListViewPage : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ContentDB contentDB = new ContentDB();
        private int _countdown = 2;
        private DispatcherTimer _timer;
        public ContentListViewPage()
        {
            InitializeComponent();


            Setup_timer();

            LoadData();

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
                try
                {
                    NavigationService.Navigate(new MenuPage());
                }
                catch (Exception)
                {

                }

            }
        }

        public void LoadData()
        {


            dataGrid.ItemsSource = contentDB.GetContents();

            cmbDepartment.ItemsSource = contentDB.GetDepartments();
            cmbType.ItemsSource = contentDB.GetItemType();
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmd = (ComboBox)sender;
            var item = (keyValueModel)cmd.SelectedItem;
            dataGrid.ItemsSource = contentDB.GetContents_itemType(item.Value);
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmd = (ComboBox)sender;
            var item = (keyValueModel)cmd.SelectedItem;
            dataGrid.ItemsSource = contentDB.GetContents_department(item.Value);
        }
    }
}
