using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpertApp2.DB
{
    public class ContentDB
    {
        public void CreateContent(ContentModel Content)
        {
            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                // 创建表
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS Item_TB (
                                        Item_Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Item_Name TEXT NOT NULL,
										Item_Description TEXT NULL,
										Charge1 TEXT NOT NULL,
										Charge2 TEXT NOT NULL,
										Row_Id TEXT NOT NULL,
										Department_Id TEXT NOT NULL,
										Is_alert INTEGER NOT NULL,
										On_hand TEXT  NULL,
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

        public void DropContent(ContentModel Content)
        {
            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                // 创建表
                string createTableQuery = @"DROP TABLE IF EXISTS Item_TB";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertContent(ContentModel Content)
        {
            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Item_TB (Item_Name, Item_Description,Charge1,Charge2,  Row_Id, Department_Id,Is_alert,On_hand, Create_By, Create_On, Update_By, Update_On) " +
                    "VALUES (@Item_Name, @Item_Description,@Charge1,@Charge2,  @Row_Id, @Department_Id,@Is_alert,@On_hand, @CreateBy, @CreateOn, @UpdateBy, @UpdateOn)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Item_Name", Content.Item_Name);
                    command.Parameters.AddWithValue("@Item_Description", Content.Item_Description);
                    command.Parameters.AddWithValue("@Charge1", Content.Charge1);
                    command.Parameters.AddWithValue("@Charge2", Content.Charge2);
                    command.Parameters.AddWithValue("@Row_Id", Content.Row_Id);
                    command.Parameters.AddWithValue("@Department_Id", Content.Department_Id);
                    command.Parameters.AddWithValue("@Is_alert", Content.Is_alert);
                    command.Parameters.AddWithValue("@On_hand", Content.On_hand);
                    command.Parameters.AddWithValue("@CreateBy", Content.CreateBy);
                    command.Parameters.AddWithValue("@CreateOn", Content.CreateOn);
                    command.Parameters.AddWithValue("@UpdateBy", Content.UpdateBy);
                    command.Parameters.AddWithValue("@UpdateOn", Content.UpdateOn);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<ContentModel> GetContents()
        {
            var Contents = new List<ContentModel>();

            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Item_TB";
                using (var command = new SQLiteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var Content = new ContentModel
                        {
                            Item_Id = Convert.ToInt32(reader["Item_Id"]),
                            Item_Name = reader["Item_Name"].ToString(),
                            Item_Description = reader["Item_Description"].ToString(),
                            Charge1 = reader["Charge1"].ToString(),
                            Charge2 = reader["Charge2"].ToString(),
                            Row_Id = reader["Row_Id"].ToString(),
                            Department_Id = reader["Department_Id"].ToString(),
                            Is_alert = Convert.ToInt32(reader["Is_alert"]),
                            On_hand = reader["On_hand"].ToString(),
                            CreateBy = reader["Create_By"].ToString(),
                            CreateOn = reader["Create_On"].ToString(),
                            UpdateBy = reader["Update_By"].ToString(),
                            UpdateOn = reader["Update_On"].ToString()
                        };
                        Contents.Add(Content);
                    }
                }
            }

            return Contents;
        }

        public void UpdateContent(ContentModel Content)
        {
            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                string sql = "UPDATE Item_TB " +
                    "SET Item_Name = @Item_Name, Item_Description = @Item_Description," +
                    "Charge1 = @Charge1, Charge2 = @Charge2, " +
                    "Row_Id = @Row_Id, Department_Id = @Department_Id, " +
                     "Is_alert = @Is_alert, On_hand = @On_hand, " +
                    "Update_By = @UpdateBy, Update_On = @UpdateOn WHERE Item_Id = @Item_Id";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Item_Name", Content.Item_Name);
                    command.Parameters.AddWithValue("@Item_Description", Content.Item_Description);
                    command.Parameters.AddWithValue("@Charge1", Content.Charge1);
                    command.Parameters.AddWithValue("@Charge2", Content.Charge2);
                    command.Parameters.AddWithValue("@Row_Id", Content.Row_Id);
                    command.Parameters.AddWithValue("@Department_Id", Content.Department_Id);
                    command.Parameters.AddWithValue("@Is_alert", Content.Is_alert);
                    command.Parameters.AddWithValue("@On_hand", Content.On_hand);
                    command.Parameters.AddWithValue("@CreateBy", Content.CreateBy);
                    command.Parameters.AddWithValue("@CreateOn", Content.CreateOn);
                    command.Parameters.AddWithValue("@UpdateBy", Content.UpdateBy);
                    command.Parameters.AddWithValue("@UpdateOn", Content.UpdateOn);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteContent(int Item_Id)
        {
            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                string sql = "DELETE FROM Item_TB WHERE Item_Id = @Item_Id";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Item_Id", Item_Id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public class ContentModel
    {
        public int Item_Id { get; set; }
        public string Item_Name { get; set; }
        public string Item_Description { get; set; }
        public string Charge1 { get; set; }
        public string Charge2 { get; set; }
        public string Row_Id { get; set; }
        public string Department_Id { get; set; }
        public string Interval { get; set; }
        public int Is_alert { get; set; }
        public string On_hand { get; set; }
        public string CreateBy { get; set; }
        public string CreateOn { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateOn { get; set; }
    }
}
