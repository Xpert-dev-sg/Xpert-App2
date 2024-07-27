using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Transactions;
using System.Data.Common;

namespace XpertApp2.DB
{
    public class UserDB
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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

        public void InsertUser(UserModel user)
        {
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
                                log.Debug($"{sql}-[{obj}]");
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
                                log.Debug($"{sql}-[{i}]");
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


            return users;
        }

        public void UpdateUser(UserModel user)
        {
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
                                command.Parameters.AddWithValue("@UpdateOn", user.UpdateOn);

                                var obj = command.ExecuteScalar();
                                log.Debug($"{sql}-[{obj}]");
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

        public void DeleteUser(int userId)
        {
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
                                command.ExecuteNonQuery();
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
                                        EventDB eventDB = new EventDB();

                                        eventDB.InsertEvent(new EventModel
                                        {
                                            Event_datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                            Event_Type = "Login",
                                            Event_Description = "User Login",
                                            User_Id = DB_Base.CurrentUser.UserId.ToString(),
                                            CreateBy = "SYSTEM",
                                            CreateOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                        }, connection);

                                        result = true;
                                    }
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
                    User_Id = DB_Base.CurrentUser.UserId.ToString(),
                    CreateBy = "SYSTEM",
                    CreateOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            catch (Exception ex)
            {
                log.Error($"CreateContent Error: {ex.Message}");
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
