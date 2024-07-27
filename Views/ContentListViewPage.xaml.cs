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
        public ContentListViewPage()
        {
            InitializeComponent();

            
            TimeUtility.CarouselMenuTimer();
            

            LoadData();

        }

        public void LoadData()
        {
            
            
            dataGrid.ItemsSource=contentDB.GetContents();

            cmbDepartment.ItemsSource = contentDB.GetDepartments();
            cmbType.ItemsSource = contentDB.GetItemType();
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmd=(ComboBox)sender;
            var item = (keyValueModel)cmd.SelectedItem;
            dataGrid.ItemsSource= contentDB.GetContents_itemType(item.Value);
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmd = (ComboBox)sender;
            var item = (keyValueModel)cmd.SelectedItem;
            dataGrid.ItemsSource = contentDB.GetContents_department(item.Value);
        }
    }
}
