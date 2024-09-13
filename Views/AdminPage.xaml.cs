using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace XpertApp2.Views
{
    /// <summary>
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class AdminPage : Page, IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ContentDB contentDB = new ContentDB();
        private UserDB userDB = new UserDB();
        private EventDB logDB = new EventDB();
        public static ObservableCollection<string> reults;
        private StringBuilder cardData;
        public AdminPage()
        {
            InitializeComponent();
            MonitorKeyMouseUntility.MonitorKeyMouseMain();
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(10);
            //timer.Tick += AfterLoginFormTimer_Tick;
            //timer.Start();
            Load();
            reults = new ObservableCollection<string>();
            lbResults.ItemsSource = reults;
            cardData = new StringBuilder();
            // 窗口加载时开始监听键盘输入 (假设读卡器模拟键盘输入)
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        private void Load()
        {
            //item

            itemdataGrid.ItemsSource = contentDB.GetContents();
            cmbDepartment.ItemsSource = contentDB.GetDepartments();
            cmbType.ItemsSource = contentDB.GetItemType();
            //user
            userdataGrid.ItemsSource = userDB.GetUsers();
            cmbrow_user.ItemsSource = userDB.GetUsers_row();
            cmbDepartment_user.ItemsSource = userDB.GetUsers_Department();

            //log
            eventdataGrid.ItemsSource = logDB.GetEvents();
            cmbType_log.ItemsSource = logDB.GetEvents_type();

            //borrow
            borrowdataGrid.ItemsSource = logDB.GetBorrowRecords();
            cmbuser_log.ItemsSource = logDB.GetBorrowRecords_user();
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
            itemForm.Closed += itemform_Closed;
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

        #region borrow
        private void cmbuser_log_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmd = (System.Windows.Controls.ComboBox)sender;
            var item = (keyValueModel)cmd.SelectedItem;
            borrowdataGrid.ItemsSource = logDB.GetBorrowRecords_user(item.Value);
        }

        private void cmbborrow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmd = (System.Windows.Controls.ComboBox)sender;
            var item = cmd.SelectedIndex;
            if (item == 0)
            {
                borrowdataGrid.ItemsSource = logDB.GetBorrowRecords_return();
            }
            else if (item == 1)
            {
                borrowdataGrid.ItemsSource = logDB.GetBorrowRecords_noreturn();
            }
            
        }

        private void txtName_borrow_SearchStarted(object sender, HandyControl.Data.FunctionEventArgs<string> e)
        {
            var sb = (SearchBar)sender;
            borrowdataGrid.ItemsSource = logDB.GetBorrowRecords(sb.Text);
        }




        #endregion

        #region test control

        private void btnReadRFID_Click(object sender, RoutedEventArgs e)
        {

            RFIDUtility rFIDUtility = new RFIDUtility();
            string s = rFIDUtility.Read_RFID();


            reults.Add(s);
        }

        private void btnOPenDoor_left_Click(object sender, RoutedEventArgs e)
        {
            string s = "574B4C590901820281";
            //reults.Add($"send {s}");
            DoorUtility doorUtility = new DoorUtility();
            doorUtility.OpenDoor_test(s);

        }

        private void btnOPenDoor_all_Click(object sender, RoutedEventArgs e)
        {
            string s = "574B4C5908018686";
            //reults.Add($"send {s}");
            DoorUtility doorUtility = new DoorUtility();
            doorUtility.OpenDoor_test(s);
        }

        private void btnOPenDoor_right_Click(object sender, RoutedEventArgs e)
        {
            string s = "574B4C590901820B88";
            //reults.Add($"send {s}");
            DoorUtility doorUtility = new DoorUtility();
            doorUtility.OpenDoor_test(s);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }



        private void btnClearRFID_Click(object sender, RoutedEventArgs e)
        {
            reults.Clear();
        }


        // 键盘输入事件监听
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
                        // 将卡片数据添加到 ListBox
                        reults.Add(cardString);

                        // 清空 StringBuilder 以便接收下一张卡
                        //cardData.Clear();
                    }
                    e.Handled = true;  // 阻止 Enter 事件继续传播
                }
                else
                {
                    // 获取按下的字符并追加到 cardData 中
                    //if (e.Key >= Key.D0 && e.Key <= Key.Z || e.Key == Key.Space)
                    {
                        // 获取字符并添加到 StringBuilder
                        string inputChar = new KeyConverter().ConvertToString(e.Key);
                        cardData.Append(inputChar);
                    }
                }
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show("Error"+ex.Message);
            }
           
        }
        #endregion

        //private void AfterLoginFormTimer_Tick(object sender, EventArgs e)
        //{
        //    if (!DB_Base.Islogined)
        //    {
        //        try
        //        {
        //            //Dispose();

        //            NavigationService.RemoveBackEntry();
        //            NavigationService.Navigate(new MenuPage());
        //        }
        //        catch (Exception)
        //        {

        //            //throw;
        //        }


        //    }

        //}

        public void Dispose()
        {
            //NavigationService.RemoveBackEntry();
            throw new NotImplementedException();
        }

        
    }
}
