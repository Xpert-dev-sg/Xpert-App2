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
using XpertApp2.DB;
using XpertApp2.Utility;

namespace XpertApp2.Views
{
    /// <summary>
    /// Interaction logic for ItemForm.xaml
    /// </summary>
    public partial class ItemForm : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ContentDB contentDB = new ContentDB();
        public static bool Is_Update = false;
        public ItemForm()
        {

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            MonitorKeyMouseUntility.MonitorKeyMouseMain();
            TimeUtility.CarouselMenuTimer();
            initialize();
        }
        #region initialize
        private void initialize()
        {
            txtDeadline.Clear();
            txtDepartment.Clear();
            txtItemDescription.Clear();
            txtItemName.Clear();
            txtItemOwner.Clear();
            txtItemPermition.Clear();
            txtItemType.Clear();
            txtRFID.Clear();
        }

        public void Update_state_Load(ContentModel item)
        {
            txtDeadline.Text = item.Interval;
            txtDepartment.Text = item.Department_Id;
            txtItemDescription.Text = item.Department_Id;
            txtItemName.Text = item.Item_Name;
            txtItemOwner.Text = item.Charge1;
            txtItemPermition.Text = item.Row_Id;
            txtItemType.Text = item.Item_type;
            txtRFID.Text = item.RFID;
            txtAlert.SelectedIndex = item.Is_alert;
        }
        #endregion

        private ContentModel GetItem_form()
        {
            var item = new ContentModel();
            item.Item_Name = txtItemName.Text;
            item.Department_Id = txtItemDescription.Text;
            item.Item_type = txtItemType.Text;
            item.Department_Id = txtDepartment.Text;
            item.Charge1 = txtItemOwner.Text;
            item.Row_Id = txtItemPermition.Text;
            item.RFID = txtRFID.Text;
            item.Interval = txtDeadline.Text;

            item.Is_alert= (int)txtAlert.SelectedIndex;
           
            item.CreateBy= DB_Base.CurrentUser.UserName;
            item.UpdateBy= DB_Base.CurrentUser.UserName;
            return item;
        }

        private bool IsFormValid()
        {
            if (string.IsNullOrEmpty(txtItemName.Text))
            {
                MessageBox.Show("Item Name is required");
                return false;
            }
            else
            {
                if (!contentDB.CheckItemName(txtItemName.Text))
                {
                    MessageBox.Show("Item Name already in DB");
                    return false;
                }

            }
            if (string.IsNullOrEmpty(txtItemDescription.Text))
            {
                MessageBox.Show("Item Description is required");
                return false;
            }
            if (string.IsNullOrEmpty(txtItemType.Text))
            {
                MessageBox.Show("Item Type is required");
                return false;
            }
            if (string.IsNullOrEmpty(txtDepartment.Text))
            {
                MessageBox.Show("Department is required");
                return false;
            }
            if (string.IsNullOrEmpty(txtItemOwner.Text))
            {
                MessageBox.Show("Item Owner is required");
                return false;
            }
            if (string.IsNullOrEmpty(txtItemPermition.Text))
            {
                MessageBox.Show("Item Permition is required");
                return false;
            }
           
            if (string.IsNullOrEmpty(txtDeadline.Text))
            {
                MessageBox.Show("Deadline is required");
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(IsFormValid())
            {
                var item = GetItem_form();
                if (Is_Update)
                {
                    
                    if (contentDB.UpdateContent(item))
                    {
                        MessageBox.Show("Item updated successfully");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Item not updated");
                    }
                }
                else
                {
                    
                    if (contentDB.InsertContent(item))
                    {
                        MessageBox.Show("Item added successfully");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Item not added");
                    }
                }
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
