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
using XpertApp2.DB;
using XpertApp2.Models;
using XpertApp2.Utility;

namespace XpertApp2.Views
{
    /// <summary>
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ContentDB contentDB = new ContentDB();
        private UserDB userDB = new UserDB();
        private EventDB logDB = new EventDB();
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

            itemdataGrid.ItemsSource = contentDB.GetContents();
            cmbDepartment.ItemsSource = contentDB.GetDepartments();
            cmbType.ItemsSource = contentDB.GetItemType();
            //user
            userdataGrid.ItemsSource = userDB.GetUsers();


            //log
            eventdataGrid.ItemsSource = logDB.GetEvents();

        }
        

        #region user
        #region search
        private void cmbType_user_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmd = (System.Windows.Controls.ComboBox)sender;
            var item = (keyValueModel)cmd.SelectedItem;
            userdataGrid.ItemsSource = userDB.GetUsers_row(item.Value);
        }

        private void cmbDepartment_user_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmd = (System.Windows.Controls.ComboBox)sender;
            var item = (keyValueModel)cmd.SelectedItem;
            userdataGrid.ItemsSource = userDB.GetUsers_department(item.Value);
        }

        private void txtItemName_user_SearchStarted(object sender, HandyControl.Data.FunctionEventArgs<string> e)
        {
            var sb = (SearchBar)sender;
            userdataGrid.ItemsSource = userDB.GetUsers_name(sb.Text);
        }
        #endregion
        private void btnnew_user_Click(object sender, RoutedEventArgs e)
        {
            UserForm userForm = new UserForm();
            UserForm.Is_Update = false;
            userForm.Closed += userform_Closed;
            userForm.ShowDialog();
        }
        private void userform_Closed(object? sender, EventArgs e)
        {
            userdataGrid.ItemsSource = userDB.GetUsers();
        }

        private void updateUser_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                int id = Convert.ToInt32(btn.Tag);
                UserForm userForm = new UserForm();
                UserForm.Is_Update = true;
                userForm.Update_state_Load(userDB.GetUsers_id(id));
                userForm.Closed += userform_Closed;
                userForm.ShowDialog();
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                int id = Convert.ToInt32(btn.Tag);
                if (userDB.DeleteUser(id))
                {
                    System.Windows.MessageBox.Show("Item deleted successfully");
                    userdataGrid.ItemsSource = userDB.GetUsers();
                }

            }
        }
        #endregion

        #region item
        #region search
        private void txtItemName_SearchStarted(object sender, HandyControl.Data.FunctionEventArgs<string> e)
        {
            var sb = (SearchBar)sender;
            itemdataGrid.ItemsSource = contentDB.GetContents_name(sb.Text);
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmd = (System.Windows.Controls.ComboBox)sender;
            var item = (keyValueModel)cmd.SelectedItem;
            itemdataGrid.ItemsSource = contentDB.GetContents_itemType(item.Value);
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmd = (System.Windows.Controls.ComboBox)sender;
            var item = (keyValueModel)cmd.SelectedItem;
            itemdataGrid.ItemsSource = contentDB.GetContents_department(item.Value);
        }


        #endregion

        private void btnnew_item_Click(object sender, RoutedEventArgs e)
        {
            ItemForm itemForm = new ItemForm(); 
            ItemForm.Is_Update = false;
            itemForm.Closed +=itemform_Closed;
            itemForm.ShowDialog();
        }
        private void itemform_Closed(object? sender, EventArgs e)
        {
            itemdataGrid.ItemsSource = contentDB.GetContents();
        }

        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                int id = Convert.ToInt32(btn.Tag);
                ItemForm itemForm = new ItemForm();
                ItemForm.Is_Update = true;
                itemForm.Update_state_Load(contentDB.GetContent_id(id));
                itemForm.Closed += itemform_Closed;
                itemForm.ShowDialog();
            }

        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                int id = Convert.ToInt32(btn.Tag);
                if (contentDB.DeleteContent(id))
                {
                    System.Windows.MessageBox.Show("Item deleted successfully");
                    itemdataGrid.ItemsSource = contentDB.GetContents();
                }

            }
        }
        #endregion

        #region log
        private void cmbType_log_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmd = (System.Windows.Controls.ComboBox)sender;
            var item = (keyValueModel)cmd.SelectedItem;
            eventdataGrid.ItemsSource = logDB.GetEvents_type(item.Value);
        }

        private void txtName_log_SearchStarted(object sender, HandyControl.Data.FunctionEventArgs<string> e)
        {
            var sb = (SearchBar)sender;
            eventdataGrid.ItemsSource = logDB.GetEvents_description(sb.Text);
        }




        #endregion

        
    }
}
