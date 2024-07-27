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
using XpertApp2.Utility;

namespace XpertApp2.Views
{
    /// <summary>
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public AdminPage()
        {
            InitializeComponent();
            MonitorKeyMouseUntility.MonitorKeyMouseMain();
            TimeUtility.CarouselMenuTimer();
            Load();
        }

        private void Load()
        {
            //item
            ContentDB contentDB = new ContentDB();
            itemdataGrid.ItemsSource= contentDB.GetContents();

            //user
            UserDB userDB = new UserDB();
            userdataGrid.ItemsSource = userDB.GetUsers();

            //log
            EventDB logDB = new EventDB();
            eventdataGrid.ItemsSource = logDB.GetEvents();
             
        }
    }
}
