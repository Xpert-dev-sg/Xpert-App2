using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpertApp2.Utility;

namespace XpertApp2.DB
{
    public class DBUtility
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void InitializeSystem()
        {
            DB_Base.DBConnectionString = ConfigurationManager.AppSettings["DBConnectionString"];
            DB_Base.SystemMail = ConfigurationManager.AppSettings["SystemMail"];
            DB_Base.SystemMenuInterval = Convert.ToInt32(ConfigurationManager.AppSettings["SystemMenuInterval"]);
            DB_Base.SystemMenuInterval_admin = Convert.ToInt32(ConfigurationManager.AppSettings["SystemMenuInterval_admin"]);
            DB_Base.mailserver = ConfigurationManager.AppSettings["mailserver"];
            DB_Base.mailport = ConfigurationManager.AppSettings["mailport"];
            DB_Base.mailusername = ConfigurationManager.AppSettings["mailusername"];
            DB_Base.mailpassword = ConfigurationManager.AppSettings["mailpassword"];
            DB_Base.door_com = ConfigurationManager.AppSettings["door_com"];
            DB_Base.door_baudrate = ConfigurationManager.AppSettings["door_baudrate"];
            DB_Base.door_parity = ConfigurationManager.AppSettings["door_parity"];
            DB_Base.door_databits = ConfigurationManager.AppSettings["door_databits"];
            DB_Base.rfid_com = ConfigurationManager.AppSettings["rfid_com"];
            DB_Base.rfid_baudrate = ConfigurationManager.AppSettings["rfid_baudrate"];
               


            #region 初始化数据库
            ContentDB contentDB = new ContentDB();
            //contentDB.CreateContent();
            UserDB userBD = new UserDB();
            //userBD.CreateUser();
            EventDB eventDB = new EventDB();
            //eventDB.CreateEvent();
            //eventDB.CreateBorrowRecords();
            //eventDB.insertTamedata();
            //contentDB.insertTamedata();
            //userBD.insertTamedata();
            #endregion

           
        }

        public static void verifyBorrow(string item)
        {

            ContentDB contentDB = new ContentDB();
            if (DB_Base.CurrentUser.RowId != "99")//admin
            {
                var sp=contentDB.GetContent_item(item);
                var item_rowid = sp.Split('-')[0];
                var item_department = sp.Split('-')[1];
                var item_isalert = sp.Split("-")[2];
                var item_charger1 = sp.Split("-")[3];
                var item_charger2 = sp.Split("-")[4];
                var msg= $"{DB_Base.CurrentUser.UserName} take {item} out on {DateTime.Now}";
                string[] tomail= new string[] { item_charger1, item_charger2,DB_Base.CurrentUser.Email };
                if (item_isalert == "1")//alert
                {
                    if (item_department != DB_Base.CurrentUser.DepartmentId)//not in the same department
                    {
                        EmailUtility.SendEmail(msg,tomail);
                        //throw new Exception("You are not allowed to borrow this item.");
                    }
                    else
                    { 
                        if(Convert.ToInt16( item_rowid)>Convert.ToInt16(DB_Base.CurrentUser.RowId))//not in the same row
                        {
                            EmailUtility.SendEmail(msg, tomail);
                            //throw new Exception("You are not allowed to borrow this item.");
                        }
                    }
                }
            }
        }
    }

    public static  class  DB_Base
    {
        public static string DBConnectionString { get; set; }//= "Data Source=XpertDB.db";

        public static UserModel CurrentUser { get; set; }

        public static bool Islogined{ get; set; }

        public static bool IsDoorOpen { get; set; }

        public static string RFIDList_c { get; set; }
        public static string RFIDList_o { get; set; }

        public static ObservableCollection<string> returnlist { get; set; }
        public static ObservableCollection<string> borrowlist { get; set; }
        public static string currentpage { get; set; }

        public static string SystemMail { get; set; }//="wangyiwater77@163.com";
        public static int SystemMenuInterval { get; set; }// = 15;
        public static int SystemMenuInterval_admin { get; set; }// = 60;

        public static string mailserver { get; set; }// = "my.smtp.exampleserver.net";
        public static string mailport { get; set; }// = "587";
        public static string mailusername { get; set; }// = "username";
        public static string mailpassword { get; set; }// = "password";
        public static string door_com { get; set; }
        public static string door_baudrate { get; set; }
        public static string door_parity { get; set; }
        public static string door_databits { get; set; }
        public static string rfid_com { get; set; }
        public static string rfid_baudrate { get; set; }
    }
}
