using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace XpertApp2.DB
{
    public class UserBD
    {
        public void CreateUser(UserModel user)
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
										Create_By TEXT NOT NULL,
										Create_On TEXT NOT NULL,
										Update_By TEXT NOT NULL,
										Update_On TEXT NOT NULL)";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DropUser(UserModel user)
        {
            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                // 创建表
                string createTableQuery = @"DROP TABLE IF EXISTS User_TB";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertUser(UserModel user)
        {
            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                string sql = "INSERT INTO User_TB (User_Name, Card_Id, Finger_Id, Row_Id, Department_Id, Create_By, Create_On, Update_By, Update_On) VALUES (@UserName, @CardId, @FingerId, @RowId, @DepartmentId, @CreateBy, @CreateOn, @UpdateBy, @UpdateOn)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserName", user.UserName);
                    command.Parameters.AddWithValue("@CardId", (object)user.CardId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FingerId", (object)user.FingerId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RowId", user.RowId);
                    command.Parameters.AddWithValue("@DepartmentId", user.DepartmentId);
                    command.Parameters.AddWithValue("@CreateBy", user.CreateBy);
                    command.Parameters.AddWithValue("@CreateOn", user.CreateOn);
                    command.Parameters.AddWithValue("@UpdateBy", user.UpdateBy);
                    command.Parameters.AddWithValue("@UpdateOn", user.UpdateOn);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<UserModel> GetUsers()
        {
            var users = new List<UserModel>();

            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM User_TB";
                using (var command = new SQLiteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new UserModel
                        {
                            UserId = Convert.ToInt32(reader["User_Id"]),
                            UserName = reader["User_Name"].ToString(),
                            CardId = reader["Card_Id"].ToString(),
                            FingerId = reader["Finger_Id"].ToString(),
                            RowId = reader["Row_Id"].ToString(),
                            DepartmentId = reader["Department_Id"].ToString(),
                            CreateBy = reader["Create_By"].ToString(),
                            CreateOn = reader["Create_On"].ToString(),
                            UpdateBy = reader["Update_By"].ToString(),
                            UpdateOn = reader["Update_On"].ToString()
                        };
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        public void UpdateUser(UserModel user)
        {
            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                string sql = "UPDATE User_TB SET User_Name = @UserName, Card_Id = @CardId, Finger_Id = @FingerId, Row_Id = @RowId, Department_Id = @DepartmentId, Update_By = @UpdateBy, Update_On = @UpdateOn WHERE User_Id = @UserId";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", user.UserId);
                    command.Parameters.AddWithValue("@UserName", user.UserName);
                    command.Parameters.AddWithValue("@CardId", (object)user.CardId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FingerId", (object)user.FingerId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RowId", user.RowId);
                    command.Parameters.AddWithValue("@DepartmentId", user.DepartmentId);
                    command.Parameters.AddWithValue("@UpdateBy", user.UpdateBy);
                    command.Parameters.AddWithValue("@UpdateOn", user.UpdateOn);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int userId)
        {
            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                string sql = "DELETE FROM User_TB WHERE User_Id = @UserId";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool IsLogined(string ps)
        {
            bool result = false;

            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM User_TB where Card_Id='{ps}'";
                using (var command = new SQLiteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            DB_Base.CurrentUser.UserId = Convert.ToInt32(reader["User_Id"]);
                            DB_Base.CurrentUser.UserName = reader["User_Name"].ToString();
                            DB_Base.CurrentUser.CardId = reader["Card_Id"].ToString();
                            DB_Base.CurrentUser.FingerId = reader["Finger_Id"].ToString();
                            DB_Base.CurrentUser.RowId = reader["Row_Id"].ToString();
                            DB_Base.CurrentUser.DepartmentId = reader["Department_Id"].ToString();

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
                            });

                            result= true;
                        }
                        
                    }
                }
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
        public string CreateBy { get; set; }
        public string CreateOn { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateOn { get; set; }
    }
}
