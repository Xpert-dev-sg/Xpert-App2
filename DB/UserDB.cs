using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Transactions;
using System.Data.Common;
using XpertApp2.Models;
using System.Windows.Interop;

namespace XpertApp2.DB
{
    public class UserDB
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EventDB eventDB = new EventDB();
        #region create, drop,insert, get, update, delete
        public void CreateUser()
        {
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    // 创建表
                    string createTableQuery = @"CREATE TABLE IF NOT EXISTS User_TB (
                                        User_Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        User_Name TEXT NOT NULL,
										Card_Id TEXT NULL,
										Finger_Id TEXT NULL,
										Row_Id TEXT NOT NULL,
										Department_Id TEXT NOT NULL,
                                        Email TEXT NOT NULL,
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
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }

        }

        public void DropUser(UserModel user)
        {
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    // 创建表
                    string createTableQuery = @"DROP TABLE IF EXISTS User_TB";
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
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }

        }

        public void insertTamedata()
        {

            UserModel user;


            user = new UserModel
            {
                UserName = $"admin user1",
                CardId = $"999",
                FingerId = $"2222",
                RowId = "999",
                DepartmentId = "admin",
                Email = "wangyiwater77@163.com",
                CreateBy = "SYSTEM",
                UpdateBy = "SYSTEM"
            };
            InsertUser(user);

            user = new UserModel
            {
                UserName = $"com user1",
                CardId = $"0",
                FingerId = $"2222",
                RowId = "1",
                DepartmentId = "operation",
                Email = "wangyiwater77@163.com",
                CreateBy = "SYSTEM",
                UpdateBy = "SYSTEM"
            };
            InsertUser(user);


            user = new UserModel
            {
                UserName = $"department user1",
                CardId = $"1",
                FingerId = $"2222",
                RowId = "10",
                DepartmentId = "operation",
                Email = "wangyiwater77@163.com",
                CreateBy = "SYSTEM",
                UpdateBy = "SYSTEM"
            };
            InsertUser(user);

            user = new UserModel
            {
                UserName = $"com user2",
                CardId = $"0",
                FingerId = $"2222",
                RowId = "1",
                DepartmentId = "account",
                Email = "wangyiwater77@163.com",
                CreateBy = "SYSTEM",
                UpdateBy = "SYSTEM"
            };
            InsertUser(user);


            user = new UserModel
            {
                UserName = $"department user2",
                CardId = $"1",
                FingerId = $"2222",
                RowId = "10",
                DepartmentId = "account",
                Email = "wangyiwater77@163.com",
                CreateBy = "SYSTEM",
                UpdateBy = "SYSTEM"
            };
            InsertUser(user);

        }

        public bool InsertUser(UserModel user)
        {
            bool result = false;
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO User_TB (User_Name, Card_Id, Finger_Id, Row_Id, Department_Id,Email, Create_By, Create_On, Update_By, Update_On) " +
                        "VALUES (@UserName, @CardId, @FingerId, @RowId, @DepartmentId,@Email, @CreateBy, @CreateOn, @UpdateBy, @UpdateOn)";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@UserName", user.UserName);
                                command.Parameters.AddWithValue("@CardId", (object)user.CardId ?? DBNull.Value);
                                command.Parameters.AddWithValue("@FingerId", (object)user.FingerId ?? DBNull.Value);
                                command.Parameters.AddWithValue("@RowId", user.RowId);
                                command.Parameters.AddWithValue("@DepartmentId", user.DepartmentId);
                                command.Parameters.AddWithValue("@Email", user.Email);
                                command.Parameters.AddWithValue("@CreateBy", user.CreateBy);
                                command.Parameters.AddWithValue("@CreateOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                command.Parameters.AddWithValue("@UpdateBy", user.UpdateBy);
                                command.Parameters.AddWithValue("@UpdateOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }
            return result;
        }

        public List<UserModel> GetUsers()
        {
            var users = new List<UserModel>();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM User_TB order by User_Name";
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
                                        var user = new UserModel
                                        {
                                            UserId = Convert.ToInt32(reader["User_Id"]),
                                            UserName = reader["User_Name"].ToString(),
                                            CardId = reader["Card_Id"].ToString(),
                                            FingerId = reader["Finger_Id"].ToString(),
                                            RowId = reader["Row_Id"].ToString(),
                                            DepartmentId = reader["Department_Id"].ToString(),
                                            Email = reader["email"].ToString(),
                                            CreateBy = reader["Create_By"].ToString(),
                                            CreateOn = reader["Create_On"].ToString(),
                                            UpdateBy = reader["Update_By"].ToString(),
                                            UpdateOn = reader["Update_On"].ToString()
                                        };
                                        users.Add(user);
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
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }


            return users;
        }

        public bool UpdateUser(UserModel user)
        {
            bool result = false;
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "UPDATE User_TB SET User_Name = @UserName, Card_Id = @CardId, Finger_Id = @FingerId, Row_Id = @RowId, Department_Id = @DepartmentId, Email=@Email,Update_By = @UpdateBy, Update_On = @UpdateOn WHERE User_Id = @UserId";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@UserId", user.UserId);
                                command.Parameters.AddWithValue("@UserName", user.UserName);
                                command.Parameters.AddWithValue("@CardId", (object)user.CardId ?? DBNull.Value);
                                command.Parameters.AddWithValue("@FingerId", (object)user.FingerId ?? DBNull.Value);
                                command.Parameters.AddWithValue("@RowId", user.RowId);
                                command.Parameters.AddWithValue("@DepartmentId", user.DepartmentId);
                                command.Parameters.AddWithValue("@Email", user.Email);
                                command.Parameters.AddWithValue("@UpdateBy", user.UpdateBy);
                                command.Parameters.AddWithValue("@UpdateOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

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
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }
            return result;
        }

        public bool DeleteUser(int userId)
        {
            bool result = false;
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM User_TB WHERE User_Id = @UserId";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@UserId", userId);
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
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }
            return result;
        }

        #endregion

        public bool IsLogined(string ps)
        {
            bool result = false;
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM User_TB where Card_Id='{ps}'";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                    while (reader.Read())
                                    {
                                        DB_Base.CurrentUser = new UserModel();
                                        DB_Base.CurrentUser.UserId = Convert.ToInt32(reader["User_Id"]);
                                        DB_Base.CurrentUser.UserName = reader["User_Name"].ToString();
                                        DB_Base.CurrentUser.CardId = reader["Card_Id"].ToString();
                                        DB_Base.CurrentUser.FingerId = reader["Finger_Id"].ToString();
                                        DB_Base.CurrentUser.RowId = reader["Row_Id"].ToString();
                                        DB_Base.CurrentUser.DepartmentId = reader["Department_Id"].ToString();
                                        DB_Base.CurrentUser.Email = reader["email"].ToString();

                                        //add log
                                        eventDB.InsertEvent_system("Login","User Login",DB_Base.CurrentUser.UserName, connection);

                                        result = true;
                                    }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }

            return result;
        }

        public void LogoutUser()
        {
            DB_Base.CurrentUser = new UserModel();
            DB_Base.Islogined = false;
            try
            {
                //add log
                EventDB eventDB = new EventDB();
                eventDB.InsertEvent(new EventModel
                {
                    Event_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Event_Type = "Logout",
                    Event_Description = "User Logout",
                    User_Id = DB_Base.CurrentUser.UserName,
                    CreateBy = "SYSTEM",
                    CreateOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }

        }

        public List<keyValueModel> GetUsers_row()
        {
            var rows = new List<keyValueModel>();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT Row_Id FROM User_TB group by Row_Id  order by Row_Id";
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
                                        var row = new keyValueModel
                                        {
                                            Key = reader["Row_Id"].ToString(),
                                            Value = reader["Row_Id"].ToString()
                                        };
                                        rows.Add(row);
                                    }
                                var msg = $"{sql}-[{i}]";
                                eventDB.InsertEvent_system("", msg, DB_Base.CurrentUser.UserName, connection);
                                log.Debug(msg); log.Debug($"{sql}-[{i}]");
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }


            return rows;
        }

        public List<keyValueModel> GetUsers_Department()
        {
            var deps = new List<keyValueModel>();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT Department_Id FROM User_TB group by Department_Id  order by Department_Id";
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
                                        var dep = new keyValueModel
                                        {
                                            Key = reader["Department_Id"].ToString(),
                                            Value = reader["Department_Id"].ToString()
                                        };
                                        deps.Add(dep);
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
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }


            return deps;
        }

        public List<UserModel> GetUsers_row(string row)
        {
            var users = new List<UserModel>();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM User_TB where Row_Id='{row}'  order by User_Name";
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
                                        var user = new UserModel
                                        {
                                            UserId = Convert.ToInt32(reader["User_Id"]),
                                            UserName = reader["User_Name"].ToString(),
                                            CardId = reader["Card_Id"].ToString(),
                                            FingerId = reader["Finger_Id"].ToString(),
                                            RowId = reader["Row_Id"].ToString(),
                                            DepartmentId = reader["Department_Id"].ToString(),
                                            Email = reader["email"].ToString(),
                                            CreateBy = reader["Create_By"].ToString(),
                                            CreateOn = reader["Create_On"].ToString(),
                                            UpdateBy = reader["Update_By"].ToString(),
                                            UpdateOn = reader["Update_On"].ToString()
                                        };
                                        users.Add(user);
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
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }


            return users;
        }

        public List<UserModel> GetUsers_department(string department)
        {
            var users = new List<UserModel>();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM User_TB where Department_Id='{department}'  order by User_Name";
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
                                        var user = new UserModel
                                        {
                                            UserId = Convert.ToInt32(reader["User_Id"]),
                                            UserName = reader["User_Name"].ToString(),
                                            CardId = reader["Card_Id"].ToString(),
                                            FingerId = reader["Finger_Id"].ToString(),
                                            RowId = reader["Row_Id"].ToString(),
                                            DepartmentId = reader["Department_Id"].ToString(),
                                            Email = reader["email"].ToString(),
                                            CreateBy = reader["Create_By"].ToString(),
                                            CreateOn = reader["Create_On"].ToString(),
                                            UpdateBy = reader["Update_By"].ToString(),
                                            UpdateOn = reader["Update_On"].ToString()
                                        };
                                        users.Add(user);
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
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }


            return users;
        }

        public List<UserModel> GetUsers_name(string name)
        {
            var users = new List<UserModel>();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM User_TB where User_Name like '%{name}%'  order by User_Name";
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
                                        var user = new UserModel
                                        {
                                            UserId = Convert.ToInt32(reader["User_Id"]),
                                            UserName = reader["User_Name"].ToString(),
                                            CardId = reader["Card_Id"].ToString(),
                                            FingerId = reader["Finger_Id"].ToString(),
                                            RowId = reader["Row_Id"].ToString(),
                                            DepartmentId = reader["Department_Id"].ToString(),
                                            Email = reader["email"].ToString(),
                                            CreateBy = reader["Create_By"].ToString(),
                                            CreateOn = reader["Create_On"].ToString(),
                                            UpdateBy = reader["Update_By"].ToString(),
                                            UpdateOn = reader["Update_On"].ToString()
                                        };
                                        users.Add(user);
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
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }


            return users;
        }

        public UserModel GetUsers_id(int id)
        {
            var user = new UserModel();
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM User_TB where User_Id = '{id}' ";
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
                                        user = new UserModel
                                        {
                                            UserId = Convert.ToInt32(reader["User_Id"]),
                                            UserName = reader["User_Name"].ToString(),
                                            CardId = reader["Card_Id"].ToString(),
                                            FingerId = reader["Finger_Id"].ToString(),
                                            RowId = reader["Row_Id"].ToString(),
                                            DepartmentId = reader["Department_Id"].ToString(),
                                            Email = reader["email"].ToString(),
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
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }


            return user;
        }

        public bool CheckUserName(string name)
        {
            bool result = false;
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT count(*) FROM User_TB where User_Name='{name}'";
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
                            log.Error($"userdb Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"userdb Error: {ex.Message}");
            }

            return result;
        }
    }

    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CardId { get; set; }
        public string FingerId { get; set; }
        public string RowId { get; set; }
        public string DepartmentId { get; set; }
        public string Email { get; set; }
        public string CreateBy { get; set; }
        public string CreateOn { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateOn { get; set; }
    }
}
