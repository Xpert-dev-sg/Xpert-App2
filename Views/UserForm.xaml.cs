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
    /// Interaction logic for UserForm.xaml
    /// </summary>
    public partial class UserForm : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private UserDB userDB = new UserDB();
        public static bool Is_Update = false;
        private int _user_id;
        public UserForm()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            MonitorKeyMouseUntility.MonitorKeyMouseMain();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += CarouselTimer_Tick;
            timer.Start();
            initialize();
        }
        #region initialize
        private void initialize()
        {
            txtuserName.Clear();
            txtemail.Clear();
            txtPermition.Clear();
            txtDepartment.Clear();
            txtcard.Clear();
            txtFinger.Clear();
        }

        public void Update_state_Load(UserModel user)
        {
            _user_id = user.UserId;
            txtuserName.Text = user.UserName;
            txtemail.Text = user.Email;
            txtPermition.Text = user.RowId;
            txtDepartment.Text = user.DepartmentId;
            txtcard.Text = user.CardId;
            txtFinger.Text = user.FingerId;
        }
        #endregion

        private UserModel GetUser_form()
        {
            var user = new UserModel();
            if (Is_Update)
                user.UserId = _user_id;
            
            user.UserName = txtuserName.Text;
            user.Email = txtemail.Text;
            user.RowId = txtPermition.Text;
            user.DepartmentId = txtDepartment.Text;
            user.CardId = txtcard.Text;
            user.FingerId = txtFinger.Text;
            user.CreateBy = DB_Base.CurrentUser.UserName;
            user.UpdateBy = DB_Base.CurrentUser.UserName;
            return user;
        }

        private bool IsFormValid()
        {
            if (string.IsNullOrEmpty(txtuserName.Text))
            {
                HandyControl.Controls.MessageBox.Show("User Name is required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                if (!Is_Update && !userDB.CheckUserName(txtuserName.Text))
                {
                    MessageBox.Show("User Name already in DB");
                    return false;
                }

            }
            if (string.IsNullOrEmpty(txtemail.Text))
            {
                HandyControl.Controls.MessageBox.Show("Email is required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (string.IsNullOrEmpty(txtPermition.Text))
            {
                HandyControl.Controls.MessageBox.Show("Permition is required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (string.IsNullOrEmpty(txtDepartment.Text))
            {
                HandyControl.Controls.MessageBox.Show("Department is required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormValid())
            {
                var user = GetUser_form();
                if (Is_Update)
                {

                    if (userDB.UpdateUser(user))
                    {
                        MessageBox.Show("User updated successfully");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("User not updated");
                    }
                }
                else
                {

                    if (userDB.InsertUser(user))
                    {
                        MessageBox.Show("User added successfully");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("User not added");
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void CarouselTimer_Tick(object sender, EventArgs e)
        {
            if (!DB_Base.Islogined)
            {
                this.Close();
            }

        }

    }
}
