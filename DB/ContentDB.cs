using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using XpertApp2.Models;

namespace XpertApp2.DB
{
    public class ContentDB
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private EventDB eventDB = new EventDB();
        #region create, drop,insert, get, update, delete
        public void CreateContent()
        {
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    // 创建表
                    string createTableQuery = @"CREATE TABLE IF NOT EXISTS Item_TB (
                                        Item_Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Item_Name TEXT NOT NULL,
										Item_Description TEXT NULL,
                                        Item_type TEXT NOT NULL,
										Charge1 TEXT  NULL,
										Charge2 TEXT  NULL,
										Row_Id TEXT NOT NULL,
										Department_Id TEXT NOT NULL,
										Is_alert INTEGER NOT NULL,
										On_hand TEXT  NULL,
                                        Interval TEXT  NULL,
                                        RFID TEXT  NULL,
										Create_By TEXT NOT NULL,
										Create_On TEXT NOT NULL,
										Update_By TEXT NOT NULL,
										Update_On TEXT NOT NULL)";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(createTableQuery, connection))
                            {
                                var obj = command.ExecuteScalar();
                                log.Debug($"{createTableQuery}-[{obj}]");
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception exx)
            {

                log.Error($"CreateContent Error: {exx.Message}");
            }



        }

        public void DropContent(ContentModel Content)
        {
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    // 创建表
                    string createTableQuery = @"DROP TABLE IF EXISTS Item_TB";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(createTableQuery, connection))
                            {
                                var obj = command.ExecuteScalar();
                                log.Debug($"{createTableQuery}-[{obj}]");
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }

        }

        public void insertTamedata()
        {
            var t = "";
            var d = "";
            for (int i = 0; i < 100; i++)
            {

                switch (i)
                {
                    case 0:
                        t = "A";
                        d = "operation";
                        break;
                    case 10:
                        t = "B";
                        d = "account";
                        break;
                    case 20:
                        t = "C";
                        d = "management";
                        break;
                    case 30:
                        t = "D";
                        d = "factory";
                        break;
                    case 40:
                        t = "E";
                        d = "driver";
                        break;
                    case 50:
                        t = "F";
                        d = "Food";
                        break;
                    case 60:
                        t = "G";
                        d = "EE";
                        break;
                    case 70:
                        t = "H";
                        d = "doctor";
                        break;
                    case 80:
                        t = "I";
                        d = "trainee";
                        break;


                }
                ContentModel Content = new ContentModel
                {
                    Item_Name = $"item{i}",
                    Item_Description = $"item{i} Description",
                    Item_type = $"{t}",
                    Charge1 = DB_Base.SystemMail,
                    Charge2 = DB_Base.SystemMail,
                    Row_Id = "1",
                    Department_Id = $"{d}",
                    Interval = "1",
                    Is_alert = 1,
                    CreateBy = "System",
                    UpdateBy = "System",
                };
                InsertContent(Content);

            }

        }

        public bool InsertContent(ContentModel Content)
        {
            bool result = false;
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Item_TB (Item_Name, Item_Description,Item_type,Charge1,Charge2,RFID,  Row_Id, Department_Id,Is_alert,Interval, Create_By, Create_On, Update_By, Update_On) " +
                        "VALUES (@Item_Name, @Item_Description,@Item_type,@Charge1,@Charge2,@RFID,  @Row_Id, @Department_Id,@Is_alert,@Interval, @CreateBy, @CreateOn, @UpdateBy, @UpdateOn)";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@Item_Name", Content.Item_Name);
                                command.Parameters.AddWithValue("@Item_Description", Content.Item_Description);
                                command.Parameters.AddWithValue("@Item_type", Content.Item_type);
                                command.Parameters.AddWithValue("@Charge1", Content.Charge1);
                                command.Parameters.AddWithValue("@Charge2", Content.Charge2);
                                command.Parameters.AddWithValue("@Row_Id", Content.Row_Id);
                                command.Parameters.AddWithValue("@Department_Id", Content.Department_Id);
                                command.Parameters.AddWithValue("@Is_alert", Content.Is_alert);
                                //command.Parameters.AddWithValue("@On_hand", Content.On_hand);
                                command.Parameters.AddWithValue("@RFID", Content.RFID);
                                command.Parameters.AddWithValue("@Interval", Content.Interval);
                                command.Parameters.AddWithValue("@CreateBy", Content.CreateBy);
                                command.Parameters.AddWithValue("@CreateOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                command.Parameters.AddWithValue("@UpdateBy", Content.UpdateBy);
                                command.Parameters.AddWithValue("@UpdateOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                                var obj = command.ExecuteScalar();
                                var msg = $"{sql}-[{obj}]";
                                if (DB_Base.CurrentUser != null)
                                    eventDB.InsertEvent_system("", msg, DB_Base.CurrentUser.UserName, connection);
                                else
                                    eventDB.InsertEvent_system("", msg, "System", connection);
                                log.Debug(msg);
                            }
                            transaction.Commit();
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }
            return result;
        }

        public List<ContentModel> GetContents()
        {
            var Contents = new List<ContentModel>();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Item_TB order by Item_Name";

                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            using (var reader = command.ExecuteReader())
                            {
                                int i = 0;
                                if (reader.HasRows)
                                    while (reader.Read())
                                    {
                                        i++;
                                        var Content = new ContentModel
                                        {
                                            Item_Id = Convert.ToInt32(reader["Item_Id"]),
                                            Item_Name = reader["Item_Name"].ToString(),
                                            Item_Description = reader["Item_Description"].ToString(),
                                            Item_type = reader["Item_type"].ToString(),
                                            Charge1 = reader["Charge1"].ToString(),
                                            Charge2 = reader["Charge2"].ToString(),
                                            Row_Id = reader["Row_Id"].ToString(),
                                            Department_Id = reader["Department_Id"].ToString(),
                                            Is_alert = Convert.ToInt32(reader["Is_alert"]),
                                            On_hand = reader["On_hand"].ToString(),
                                            Interval = reader["Interval"].ToString(),
                                            CreateBy = reader["Create_By"].ToString(),
                                            CreateOn = reader["Create_On"].ToString(),
                                            UpdateBy = reader["Update_By"].ToString(),
                                            UpdateOn = reader["Update_On"].ToString()
                                        };
                                        Contents.Add(Content);
                                    }
                                var msg = $"{sql}-[{i}]";
                                var user = "";
                                if(DB_Base.CurrentUser==null)
                                    user= "System";
                                else
                                    user = DB_Base.CurrentUser.UserName;

                                eventDB.InsertEvent_system("", msg, user, connection);
                                log.Debug(msg);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }


            return Contents;
        }

        public bool UpdateContent(ContentModel Content)
        {
            bool result = false;
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Item_TB " +
                        "SET Item_Name = @Item_Name, Item_Description = @Item_Description," +
                        "Charge1 = @Charge1, Charge2 = @Charge2, Item_type=@Item_type," +
                        "Row_Id = @Row_Id, Department_Id = @Department_Id, " +
                         "Is_alert = @Is_alert,  Interval=@Interval,RFID=@RFID," +//On_hand = @On_hand,
                        "Update_By = @UpdateBy, Update_On = @UpdateOn WHERE Item_Id = @Item_Id";

                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@Item_Name", Content.Item_Name);
                                command.Parameters.AddWithValue("@Item_Description", Content.Item_Description);
                                command.Parameters.AddWithValue("@Item_type", Content.Item_type);
                                command.Parameters.AddWithValue("@Charge1", Content.Charge1);
                                command.Parameters.AddWithValue("@Charge2", Content.Charge2);
                                command.Parameters.AddWithValue("@Row_Id", Content.Row_Id);
                                command.Parameters.AddWithValue("@Department_Id", Content.Department_Id);
                                command.Parameters.AddWithValue("@Is_alert", Content.Is_alert);
                                //command.Parameters.AddWithValue("@On_hand", Content.On_hand);
                                command.Parameters.AddWithValue("@RFID", Content.RFID);
                                command.Parameters.AddWithValue("@Interval", Content.Interval);
                                command.Parameters.AddWithValue("@UpdateBy", Content.UpdateBy);
                                command.Parameters.AddWithValue("@UpdateOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                command.Parameters.AddWithValue("@Item_Id", Content.Item_Id);

                                var obj = command.ExecuteScalar();
                                var msg = $"{sql}-[{obj}]";
                                eventDB.InsertEvent_system("", msg, DB_Base.CurrentUser.UserName, connection);
                                log.Debug(msg);
                                result = true;
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }
            return result;
        }

        public bool DeleteContent(int Item_Id)
        {
            bool result = false;
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM Item_TB WHERE Item_Id = @Item_Id";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@Item_Id", Item_Id);
                                var obj = command.ExecuteScalar();
                                var msg = $"{sql}-[{obj}]";
                                eventDB.InsertEvent_system("", msg, DB_Base.CurrentUser.UserName, connection);
                                log.Debug(msg);
                            }
                            transaction.Commit();
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }
            return result;
        }
        #endregion

        public List<keyValueModel> GetDepartments()
        {
            var Departments = new List<keyValueModel>();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT  Department_Id  FROM Item_TB group by Department_Id order by Department_Id";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {

                            using (var command = new SQLiteCommand(sql, connection))
                            using (var reader = command.ExecuteReader())
                            {
                                int i = 0;
                                while (reader.Read())
                                {
                                    i++;
                                    var Department = new keyValueModel
                                    {
                                        Key = reader["Department_Id"].ToString(),
                                        Value = reader["Department_Id"].ToString()
                                    };
                                    Departments.Add(Department);
                                }
                                var msg = $"{sql}-[{i}]";
                                var user = "";
                                if (DB_Base.CurrentUser == null)
                                    user = "System";
                                else
                                    user = DB_Base.CurrentUser.UserName;
                                eventDB.InsertEvent_system("", msg, user, connection);
                                log.Debug(msg);
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }


            return Departments;
        }

        public List<keyValueModel> GetItemType()
        {
            var Departments = new List<keyValueModel>();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT  Item_type  FROM Item_TB group by Item_type order by Item_type";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {

                            using (var command = new SQLiteCommand(sql, connection))
                            using (var reader = command.ExecuteReader())
                            {
                                int i = 0;
                                while (reader.Read())
                                {
                                    i++;
                                    var Department = new keyValueModel
                                    {
                                        Key = reader["Item_type"].ToString(),
                                        Value = reader["Item_type"].ToString()
                                    };
                                    Departments.Add(Department);
                                }
                                var msg = $"{sql}-[{i}]";
                                var user = "";
                                if (DB_Base.CurrentUser == null)
                                    user = "System";
                                else
                                    user = DB_Base.CurrentUser.UserName;
                                eventDB.InsertEvent_system("", msg, user, connection);
                                log.Debug(msg);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }


            return Departments;
        }

        public List<ContentModel> GetContents_itemType(string itemType)
        {
            var Contents = new List<ContentModel>();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM Item_TB where Item_type='{itemType}' order by Item_Name";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            using (var reader = command.ExecuteReader())
                            {
                                int i = 0;
                                while (reader.Read())
                                {
                                    i++;
                                    var Content = new ContentModel
                                    {
                                        Item_Id = Convert.ToInt32(reader["Item_Id"]),
                                        Item_Name = reader["Item_Name"].ToString(),
                                        Item_Description = reader["Item_Description"].ToString(),
                                        Item_type = reader["Item_type"].ToString(),
                                        Charge1 = reader["Charge1"].ToString(),
                                        Charge2 = reader["Charge2"].ToString(),
                                        Row_Id = reader["Row_Id"].ToString(),
                                        Department_Id = reader["Department_Id"].ToString(),
                                        Is_alert = Convert.ToInt32(reader["Is_alert"]),
                                        On_hand = reader["On_hand"].ToString(),
                                        Interval = reader["Interval"].ToString(),
                                        CreateBy = reader["Create_By"].ToString(),
                                        CreateOn = reader["Create_On"].ToString(),
                                        UpdateBy = reader["Update_By"].ToString(),
                                        UpdateOn = reader["Update_On"].ToString()
                                    };
                                    Contents.Add(Content);
                                }
                                var msg = $"{sql}-[{i}]";
                                eventDB.InsertEvent_system("", msg, DB_Base.CurrentUser.UserName, connection);
                                log.Debug(msg);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }

            return Contents;
        }

        public List<ContentModel> GetContents_department(string department)
        {
            var Contents = new List<ContentModel>();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM Item_TB where Department_Id='{department}' order by Item_Name";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            using (var reader = command.ExecuteReader())
                            {
                                int i = 0;
                                while (reader.Read())
                                {
                                    i++;
                                    var Content = new ContentModel
                                    {
                                        Item_Id = Convert.ToInt32(reader["Item_Id"]),
                                        Item_Name = reader["Item_Name"].ToString(),
                                        Item_Description = reader["Item_Description"].ToString(),
                                        Item_type = reader["Item_type"].ToString(),
                                        Charge1 = reader["Charge1"].ToString(),
                                        Charge2 = reader["Charge2"].ToString(),
                                        Row_Id = reader["Row_Id"].ToString(),
                                        Department_Id = reader["Department_Id"].ToString(),
                                        Is_alert = Convert.ToInt32(reader["Is_alert"]),
                                        On_hand = reader["On_hand"].ToString(),
                                        Interval = reader["Interval"].ToString(),
                                        CreateBy = reader["Create_By"].ToString(),
                                        CreateOn = reader["Create_On"].ToString(),
                                        UpdateBy = reader["Update_By"].ToString(),
                                        UpdateOn = reader["Update_On"].ToString()
                                    };
                                    Contents.Add(Content);
                                }
                                var msg = $"{sql}-[{i}]";
                                eventDB.InsertEvent_system("", msg, DB_Base.CurrentUser.UserName, connection);
                                log.Debug(msg);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }

            return Contents;
        }

        public List<ContentModel> GetContents_name(string name)
        {
            var Contents = new List<ContentModel>();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM Item_TB where Item_Name like '%{name}%' order by Item_Id";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            using (var reader = command.ExecuteReader())
                            {
                                int i = 0;
                                while (reader.Read())
                                {
                                    i++;
                                    var Content = new ContentModel
                                    {
                                        Item_Id = Convert.ToInt32(reader["Item_Id"]),
                                        Item_Name = reader["Item_Name"].ToString(),
                                        Item_Description = reader["Item_Description"].ToString(),
                                        Item_type = reader["Item_type"].ToString(),
                                        Charge1 = reader["Charge1"].ToString(),
                                        Charge2 = reader["Charge2"].ToString(),
                                        Row_Id = reader["Row_Id"].ToString(),
                                        Department_Id = reader["Department_Id"].ToString(),
                                        Is_alert = Convert.ToInt32(reader["Is_alert"]),
                                        On_hand = reader["On_hand"].ToString(),
                                        Interval = reader["Interval"].ToString(),
                                        CreateBy = reader["Create_By"].ToString(),
                                        CreateOn = reader["Create_On"].ToString(),
                                        UpdateBy = reader["Update_By"].ToString(),
                                        UpdateOn = reader["Update_On"].ToString()
                                    };
                                    Contents.Add(Content);
                                }
                                var msg = $"{sql}-[{i}]";
                                eventDB.InsertEvent_system("", msg, DB_Base.CurrentUser.UserName, connection);
                                log.Debug(msg);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }

            return Contents;
        }

        public ContentModel GetContent_id(int id)
        {
            var Content = new ContentModel();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM Item_TB where Item_Id = '{id}' ";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            using (var reader = command.ExecuteReader())
                            {
                                int i = 0;
                                while (reader.Read())
                                {
                                    i++;
                                    Content = new ContentModel
                                    {
                                        Item_Id = Convert.ToInt32(reader["Item_Id"]),
                                        Item_Name = reader["Item_Name"].ToString(),
                                        Item_Description = reader["Item_Description"].ToString(),
                                        Item_type = reader["Item_type"].ToString(),
                                        Charge1 = reader["Charge1"].ToString(),
                                        Charge2 = reader["Charge2"].ToString(),
                                        Row_Id = reader["Row_Id"].ToString(),
                                        Department_Id = reader["Department_Id"].ToString(),
                                        Is_alert = Convert.ToInt32(reader["Is_alert"]),
                                        On_hand = reader["On_hand"].ToString(),
                                        Interval = reader["Interval"].ToString(),
                                        RFID = reader["RFID"].ToString(),
                                        CreateBy = reader["Create_By"].ToString(),
                                        CreateOn = reader["Create_On"].ToString(),
                                        UpdateBy = reader["Update_By"].ToString(),
                                        UpdateOn = reader["Update_On"].ToString()
                                    };
                                    
                                }
                                var msg = $"{sql}-[{i}]";
                                eventDB.InsertEvent_system("", msg, DB_Base.CurrentUser.UserName, connection);
                                log.Debug(msg);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }

            return Content;
        }
        public bool CheckItemName(string name)
        {
            var result = false;
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT count(*) FROM Item_TB where Item_Name ='{name}'";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            {
                                var obj = command.ExecuteScalar();
                                var msg = $"{sql}-[{obj}]";
                                eventDB.InsertEvent_system("", msg, DB_Base.CurrentUser.UserName, connection);
                                log.Debug(msg);
                                result = Convert.ToInt32(obj) == 0;
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }

            return result;
        }

        public void UpdateContent_on_hand(string  rfid,string user)
        {
           
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Item_TB " +
                        "SET On_hand = @On_hand " +
                        " WHERE RFID=@RFID";

                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            {
                               
                                command.Parameters.AddWithValue("@On_hand", user);
                                command.Parameters.AddWithValue("@RFID", rfid);
                                

                                var obj = command.ExecuteScalar();
                                var msg = $"{sql}-[{obj}]";
                                eventDB.InsertEvent_system("", msg, DB_Base.CurrentUser.UserName, connection);
                                log.Debug(msg);
                                
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }
            
        }

        public string GetContent_rfid(string rfid)
        {
            var Contents = "";
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM Item_TB where RFID = '{rfid}' ";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var Item_Name = reader["Item_Name"].ToString();
                                    var Item_Description = reader["Item_Description"].ToString();
                                    Contents= $"{Item_Name}-{Item_Description}";
                                }
                                var msg = $"{sql}-[1]";
                                eventDB.InsertEvent_system("", msg, DB_Base.CurrentUser.UserName, connection);
                                log.Debug(msg);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }

            return Contents;
        }


        public string GetContent_item(string item)
        {
            //var Item_Name= item.Split('-')[0];
            //var Item_Description = item.Split('-')[1];
            var Contents = "";
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    //string sql = $"SELECT * FROM Item_TB where Item_Name = '{Item_Name}' and Item_Description='{Item_Description}' ";
                    string sql = $"SELECT * FROM Item_TB where RFID = '{item}' ";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var Row_Id = reader["Row_Id"].ToString();
                                    var Department_Id = reader["Department_Id"].ToString();
                                    var Is_alert = Convert.ToInt32(reader["Is_alert"]);
                                    var Charge1 = reader["Charge1"].ToString();
                                    var Charge2 = reader["Charge2"].ToString();
                                    Contents = $"{Row_Id}-{Department_Id}-{Is_alert}-{Charge1}-{Charge2}";
                                }
                                var msg = $"{sql}-[1]";
                                eventDB.InsertEvent_system("", msg, DB_Base.CurrentUser.UserName, connection);
                                log.Debug(msg);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"CreateContent Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
            }

            return Contents;
        }

    }



    public class ContentModel
    {
        public int Item_Id { get; set; }
        public string Item_Name { get; set; }
        public string Item_Description { get; set; }
        public string Item_type { get; set; }
        public string Charge1 { get; set; }
        public string Charge2 { get; set; }
        public string Row_Id { get; set; }
        public string Department_Id { get; set; }
        public string Interval { get; set; }
        public int Is_alert { get; set; }
        public string On_hand { get; set; }
        public string RFID { get; set; }
        public string CreateBy { get; set; }
        public string CreateOn { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateOn { get; set; }
    }
}
