using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpertApp2.Service
{
    public  class dbcreate
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void CreateDatabase()
        {
            // 数据库文件路径
            string databasePath = "Data Source=XpertDB.db";

            // 创建数据库连接
            using (var connection = new SQLiteConnection(databasePath))
            {
                connection.Open();

                //// 创建表
                //string createTableQuery = @"DROP TABLE IF EXISTS people";
                //using (var command = new SQLiteCommand(createTableQuery, connection))
                //{
                //    command.ExecuteNonQuery();
                //}

                // 插入数据
                string insertDataQuery = "INSERT INTO Item_TB (Item_Name, Item_Description,Charge1,Charge2,Row_Id,Department_Id,Is_alert,On_hand,Create_By,Create_On,Update_By,Update_On) " +
                    "VALUES (@Name, @Age)";
                using (var command = new SQLiteCommand(insertDataQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", "Alice");
                    command.Parameters.AddWithValue("@Age", 30);
                    command.ExecuteNonQuery();
                }

                // 查询数据
                //string selectDataQuery = "SELECT * FROM people";
                //using (var command = new SQLiteCommand(selectDataQuery, connection))
                //using (var reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["Name"]}, Age: {reader["Age"]}");
                //    }
                //}
            }
        }
    }
}
//DROP TABLE IF EXISTS people

//CREATE TABLE IF NOT EXISTS User_TB (
//                                        User_Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                                        User_Name TEXT NOT NULL,
//                                        Card_Id TEXT NULL,
//                                        Finger_Id TEXT NULL,
//                                        Row_Id TEXT NOT NULL,
//                                        Department_Id TEXT NOT NULL,
//                                        Create_By TEXT NOT NULL,
//                                        Create_On TEXT NOT NULL,
//                                        Update_By TEXT NOT NULL,
//                                        Update_On TEXT NOT NULL)

//										CREATE TABLE IF NOT EXISTS Item_TB (
//                                        Item_Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                                        Item_Name TEXT NOT NULL,
//                                        Item_Description TEXT NULL,
//                                        Charge1 TEXT NOT NULL,
//                                        Charge2 TEXT NOT NULL,
//                                        Row_Id TEXT NOT NULL,
//                                        Department_Id TEXT NOT NULL,
//                                        Is_alert INTEGER NOT NULL,
//                                        On_hand TEXT  NULL,
//                                        Create_By TEXT NOT NULL,
//                                        Create_On TEXT NOT NULL,
//                                        Update_By TEXT NOT NULL,
//                                        Update_On TEXT NOT NULL)


//										CREATE TABLE IF NOT EXISTS Event_Log_TB (
//                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                                        Event_datetime TEXT NOT NULL,
//                                        Event_Type TEXT NOT NULL,
//                                        Event_Description  TEXT NOT NULL,
//                                        User_Id INTEGER NOT NULL,
//                                        Create_By TEXT NOT NULL,
//                                        Create_On TEXT NOT NULL)