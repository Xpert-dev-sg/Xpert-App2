using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Xml.Linq;
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
            DB_Base.systemservice_time = ConfigurationManager.AppSettings["systemservice_time"];



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
                var sp = contentDB.GetContent_item(item);
                var item_rowid = sp.Split('-')[0];
                var item_department = sp.Split('-')[1];
                var item_isalert = sp.Split("-")[2];
                var item_charger1 = sp.Split("-")[3];
                var item_charger2 = sp.Split("-")[4];
                var msg = $"{DB_Base.CurrentUser.UserName} take {item} out on {DateTime.Now}";
                string[] tomail = new string[] { item_charger1, item_charger2, DB_Base.CurrentUser.Email };
                var Subject = "not allowed to borrow  item";
                if (item_isalert == "1")//alert
                {
                    if (item_department != DB_Base.CurrentUser.DepartmentId)//not in the same department
                    {
                        EmailUtility.SendEmail(msg, Subject, tomail);
                        //throw new Exception("You are not allowed to borrow this item.");
                    }
                    else
                    {
                        if (Convert.ToInt16(item_rowid) > Convert.ToInt16(DB_Base.CurrentUser.RowId))//not in the same row
                        {
                            EmailUtility.SendEmail(msg, Subject, tomail);
                            //throw new Exception("You are not allowed to borrow this item.");
                        }
                    }
                }
            }
        }

        public static void verifyBorrowTime()
        {
            EventDB eventDB = new EventDB();
            ContentDB contentDB = new ContentDB();
            UserDB userDB = new UserDB();
            var borrow_noreturnlist = eventDB.GetBorrowRecords_noreturn();
            List<Borrowitem> borrowitemlist = new List<Borrowitem>();
            List<string> users = new List<string>();
            foreach (var item in borrow_noreturnlist)
            {
                var borrowtime = Convert.ToDateTime(item.take_Datetime);
                var interval = Convert.ToInt16(item.interval);
                var now = DateTime.Now;
                if ((now - borrowtime).TotalDays > interval)//more than borrow interval
                {
                    Borrowitem borrowitem = new Borrowitem();
                    borrowitem.Item_name = item.Item_name;
                    borrowitem.User_Id = item.User_Id;
                    borrowitemlist.Add(borrowitem);
                    if (!users.Contains(item.User_Id))
                        users.Add(item.User_Id);
                }

            }

            foreach (var user in users)
            {
                var stritem= "";
                var bl = borrowitemlist.Where(x => x.User_Id == user).ToList();
                foreach (var item in bl)
                {
                    stritem+=item.Item_name+",</br>";

                }
                if (!string.IsNullOrEmpty(stritem))
                {
                    var email = userDB.GetUserEmail_Name(user);
                    var msg = $"{user} has over due date item </br> {stritem} ";
                    string[] tomail = new string[] { email };
                    var Subject = "over due date  item";
                    EmailUtility.SendEmail(msg, Subject, tomail);
                }
                
            }

        }
    }

    public static class DB_Base
    {
        public static string DBConnectionString { get; set; }//= "Data Source=XpertDB.db";

        public static UserModel CurrentUser { get; set; }

        public static bool Islogined { get; set; }

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
        public static string systemservice_time { get; set; }
    }
}
