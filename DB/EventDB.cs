using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpertApp2.DB
{
    public class EventDB
    {
        public void CreateEvent(EventModel Event)
        {
            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                // 创建表
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS Event_Log_TB (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Event_datetime TEXT NOT NULL,
										Event_Type TEXT NOT NULL,
										Event_Description  TEXT NOT NULL,
										User_Id INTEGER NOT NULL,
										Create_By TEXT NOT NULL,
										Create_On TEXT NOT NULL)";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DropEvent(EventModel Event)
        {
            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                
                string createTableQuery = @"DROP TABLE IF EXISTS Event_Log_TB";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertEvent(EventModel Event)
        {
            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Event_Log_TB (Event_datetime, Event_Type,Event_Description,User_Id,  Row_Id, Department_Id,Is_alert,On_hand, Create_By, Create_On, Update_By, Update_On) " +
                    "VALUES (@Item_Name, @Item_Description,@Charge1,@Charge2,  @Row_Id, @Department_Id,@Is_alert,@On_hand, @CreateBy, @CreateOn, @UpdateBy, @UpdateOn)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Event_datetime", Event.Event_datetime);
                    command.Parameters.AddWithValue("@Event_Type", Event.Event_Type);
                    command.Parameters.AddWithValue("@Event_Description", Event.Event_Description);
                    command.Parameters.AddWithValue("@User_Id", Event.User_Id);
                    command.Parameters.AddWithValue("@CreateBy", Event.CreateBy);
                    command.Parameters.AddWithValue("@CreateOn", Event.CreateOn);
                  

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<EventModel> GetEvents()
        {
            var Events = new List<EventModel>();

            using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Event_Log_TB";
                using (var command = new SQLiteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var Event = new EventModel
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Event_datetime = reader["Event_datetime"].ToString(),
                            Event_Type = reader["Event_Type"].ToString(),
                            Event_Description = reader["Event_Description"].ToString(),
                            User_Id = reader["User_Id"].ToString(),
                            CreateBy = reader["Create_By"].ToString(),
                            CreateOn = reader["Create_On"].ToString()
                            
                        };
                        Events.Add(Event);
                    }
                }
            }

            return Events;
        }

    }

    public class EventModel
    {
        public int Id { get; set; }
        public string Event_datetime { get; set; }
        public string Event_Type { get; set; }
        public string Event_Description { get; set; }
        public string User_Id { get; set; }
        public string CreateBy { get; set; }
        public string CreateOn { get; set; }
     
    }
}
