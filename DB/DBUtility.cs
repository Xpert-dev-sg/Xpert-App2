using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpertApp2.DB
{
    public class DBUtility
    {

    }

    public static  class  DB_Base
    {
        public static string DBConnectionString { get; set; }//= "Data Source=XpertDB.db";

        public static UserModel CurrentUser { get; set; }

        public static bool Islogined{ get; set; }

        public static string SystemMail { get; set; }//="wangyiwater77@163.com";
        public static int SystemMenuInterval { get; set; }// = 15;
        public static int SystemMenuInterval_admin { get; set; }// = 60;
    }
}
