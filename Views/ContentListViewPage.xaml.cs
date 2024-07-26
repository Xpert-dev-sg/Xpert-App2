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
        public ContentListViewPage()
        {
            InitializeComponent();

            
            TimeUtility.CarouselMenuTimer();
            

            LoadData();

        }

        public void LoadData()
        {
            var data = new List<ContentModel>
            {
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                 new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},

                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id="1",Department_Id="1",Is_alert=1,On_hand="",CreateBy="",CreateOn="",UpdateBy="",UpdateOn=""},
                new ContentModel { Item_Name = "张三", Item_Description = "", Charge1 = "北京", Charge2 = "北京", Row_Id = "1", Department_Id = "1", Is_alert = 1, On_hand = "", CreateBy = "", CreateOn = "", UpdateBy = "", UpdateOn = "" }
            };

            ContentDB contentDB = new ContentDB();

            dataGrid.ItemsSource = data;//contentDB.GetContents();
        }

        
    }
}
