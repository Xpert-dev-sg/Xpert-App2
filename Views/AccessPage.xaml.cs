using System;
using System.Collections.Generic;
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
        public AccessPage()
        {
            InitializeComponent();

            MonitorKeyMouseUntility.MonitorKeyMouseMain();
            TimeUtility.CarouselMenuTimer();
            Load();
        }

        public void Load()
        {
            txtDepartment.Text = DB_Base.CurrentUser.DepartmentId;
            txtEmail.Text = DB_Base.CurrentUser.Email;
            txtusername.Text = DB_Base.CurrentUser.UserName;

            EventDB eventDB = new EventDB();
            borrowdataGrid.ItemsSource = eventDB.GetBorrowRecords_user_access(DB_Base.CurrentUser.UserName);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("open door");
        }
    }
}
