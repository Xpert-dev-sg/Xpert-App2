using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace XpertApp2.DB
{
    public class EventDB
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region event
        public void CreateEvent()
        {
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    // 创建表
                    string createTableQuery = @"CREATE TABLE IF NOT EXISTS Event_Log_TB (
                                        Id TEXT PRIMARY KEY,
                                        Event_datetime TEXT NOT NULL,
										Event_Type TEXT NOT NULL,
										Event_Description  TEXT NOT NULL,
										User_Id INTEGER NOT NULL,
										Create_By TEXT NOT NULL,
										Create_On TEXT NOT NULL)";
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
                            log.Error($"Event Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                log.Error($"Event_Log Error: {ex.Message}");
            }

        }
        public void DropEvent(EventModel Event)
        {
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();

                    string createTableQuery = @"DROP TABLE IF EXISTS Event_Log_TB";
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
                            log.Error($"Event_Log Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                log.Error($"Event_Log Error: {ex.Message}");
            }

        }


        public void InsertEvent(EventModel Event)
        {
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Event_Log_TB (Id,Event_datetime, Event_Type,Event_Description,User_Id,Create_By,Create_On) " +
                        "VALUES (@Id,@Event_datetime, @Event_Type,@Event_Description,@User_Id, @Create_By, @Create_On)";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            {
                                string id = Guid.NewGuid().ToString();
                                command.Parameters.AddWithValue("@Id", id);
                                command.Parameters.AddWithValue("@Event_datetime", Event.Event_datetime);
                                command.Parameters.AddWithValue("@Event_Type", Event.Event_Type);
                                command.Parameters.AddWithValue("@Event_Description", Event.Event_Description);
                                command.Parameters.AddWithValue("@User_Id", Event.User_Id);
                                command.Parameters.AddWithValue("@Create_By", Event.CreateBy);
                                command.Parameters.AddWithValue("@Create_On", Event.CreateOn);


                                var obj = command.ExecuteScalar();
                                log.Debug($"{sql}-[{obj}]");
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"Event_Log Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                log.Error($"Event_Log Error: {ex.Message}"); ;
            }

        }

        public void InsertEvent(EventModel Event, SQLiteConnection connection)
        {
            string sql = "INSERT INTO Event_Log_TB (Id,Event_datetime, Event_Type,Event_Description,User_Id,Create_By,Create_On) " +
                       "VALUES (@Id,@Event_datetime, @Event_Type,@Event_Description,@User_Id, @Create_By, @Create_On)";
           
                using (var command = new SQLiteCommand(sql, connection))
                {
                    string id = Guid.NewGuid().ToString();
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Event_datetime", Event.Event_datetime);
                    command.Parameters.AddWithValue("@Event_Type", Event.Event_Type);
                    command.Parameters.AddWithValue("@Event_Description", Event.Event_Description);
                    command.Parameters.AddWithValue("@User_Id", Event.User_Id);
                    command.Parameters.AddWithValue("@Create_By", Event.CreateBy);
                    command.Parameters.AddWithValue("@Create_On", Event.CreateOn);


                    var obj = command.ExecuteScalar();
                    log.Debug($"{sql}-[{obj}]");
                }

           

        }

        public List<EventModel> GetEvents()
        {
            var Events = new List<EventModel>();

            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Event_Log_TB order by Event_datetime desc";
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
                                    var Event = new EventModel
                                    {
                                        Id = reader["Id"].ToString(),
                                        Event_datetime = reader["Event_datetime"].ToString(),
                                        Event_Type = reader["Event_Type"].ToString(),
                                        Event_Description = reader["Event_Description"].ToString(),
                                        User_Id = reader["User_Id"].ToString(),
                                        CreateBy = reader["Create_By"].ToString(),
                                        CreateOn = reader["Create_On"].ToString()

                                    };
                                    Events.Add(Event);
                                }
                                log.Debug($"{sql}-[{i}]");
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"Event_Log Error: {ex.Message}");
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                log.Error($"Event_Log Error: {ex.Message}");
            }
            return Events;
        }

        public List<EventModel> GetEvents_type(string type)
        {
            var Events = new List<EventModel>();

            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM Event_Log_TB where Event_Type='{type}' order by Event_datetime desc";
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
                                    var Event = new EventModel
                                    {
                                        Id = reader["Id"].ToString(),
                                        Event_datetime = reader["Event_datetime"].ToString(),
                                        Event_Type = reader["Event_Type"].ToString(),
                                        Event_Description = reader["Event_Description"].ToString(),
                                        User_Id = reader["User_Id"].ToString(),
                                        CreateBy = reader["Create_By"].ToString(),
                                        CreateOn = reader["Create_On"].ToString()

                                    };
                                    Events.Add(Event);
                                }
                                log.Debug($"{sql}-[{i}]");
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"Event_Log Error: {ex.Message}");
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                log.Error($"Event_Log Error: {ex.Message}");
            }
            return Events;
        }

        public List<EventModel> GetEvents_description(string name)
        {
            var Events = new List<EventModel>();

            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM Event_Log_TB where Event_Description like '%{name}%' order by Event_datetime desc";
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
                                    var Event = new EventModel
                                    {
                                        Id = reader["Id"].ToString(),
                                        Event_datetime = reader["Event_datetime"].ToString(),
                                        Event_Type = reader["Event_Type"].ToString(),
                                        Event_Description = reader["Event_Description"].ToString(),
                                        User_Id = reader["User_Id"].ToString(),
                                        CreateBy = reader["Create_By"].ToString(),
                                        CreateOn = reader["Create_On"].ToString()

                                    };
                                    Events.Add(Event);
                                }
                                log.Debug($"{sql}-[{i}]");
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"Event_Log Error: {ex.Message}");
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                log.Error($"Event_Log Error: {ex.Message}");
            }
            return Events;
        }

        #endregion

        #region BorrowRecords
        public void CreateBorrowRecords()
        {
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    // 创建表
                    string createTableQuery = @"CREATE TABLE IF NOT EXISTS Borrow_Records_TB (
                                        Id TEXT PRIMARY KEY,
                                        Borrow_datetime TEXT NOT NULL,
										Return_datetime TEXT  NULL,
										item_id  TEXT NOT NULL,
										User_Id INTEGER NOT NULL,
										Create_By TEXT NOT NULL,
										Create_On TEXT NOT NULL)";
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
                            log.Error($"BorrowRecords Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                log.Error($"BorrowRecords Error: {ex.Message}");
            }

        }

        public void DropBorrowRecords(EventModel Event)
        {
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();

                    string createTableQuery = @"DROP TABLE IF EXISTS Borrow_Records_TB";
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
                            log.Error($"BorrowRecords Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                log.Error($"BorrowRecords Error: {ex.Message}");
            }

        }

        public void InsertBorrowRecords(string item_id, string user_id)
        {
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Borrow_Records_TB (Id,Borrow_datetime, Return_datetime,item_id,User_Id,Create_By,Create_On) " +
                        "VALUES (@Id,@Borrow_datetime, @Return_datetime,@item_id,@User_Id,@Create_By, @Create_On)";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {

                            using (var command = new SQLiteCommand(sql, connection))
                            {
                                string id = Guid.NewGuid().ToString();
                                command.Parameters.AddWithValue("@Id", id);
                                command.Parameters.AddWithValue("@Borrow_datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                //command.Parameters.AddWithValue("@Return_datetime", Event.Event_Type);
                                command.Parameters.AddWithValue("@item_id", item_id);
                                command.Parameters.AddWithValue("@User_Id", user_id);
                                command.Parameters.AddWithValue("@CreateBy", user_id);
                                command.Parameters.AddWithValue("@CreateOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));


                                var obj = command.ExecuteScalar();
                                log.Debug($"{sql}-[{obj}]");
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"BorrowRecords Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                log.Error($"BorrowRecords Error: {ex.Message}");
            }

        }

        public void UpdateBorrowRecords_return(string item_id, string user_id)
        {
            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Borrow_Records_TB SET  Return_datetime=@Return_datetime WHERE item_id=@item_id AND User_Id=@User_Id AND  Return_datetime = NULL";
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SQLiteCommand(sql, connection))
                            {

                                command.Parameters.AddWithValue("@item_id", item_id);
                                command.Parameters.AddWithValue("@User_Id", user_id);
                                command.Parameters.AddWithValue("@Return_datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                var obj = command.ExecuteScalar();
                                log.Debug($"{sql}-[{obj}]");
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"BorrowRecords Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                log.Error($"BorrowRecords Error: {ex.Message}");
            }

        }

        public List<BorrowModel> GetBorrowRecords(int user_id)
        {
            var BorrowRecords = new List<BorrowModel>();

            try
            {
                using (var connection = new SQLiteConnection(DB_Base.DBConnectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM Borrow_Records_TB where user_id='{user_id}' and Return_datetime is null order by Create_On desc";
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
                                    var BorrowRecord = new BorrowModel
                                    {
                                        Id = reader["Id"].ToString(),
                                        take_Datetime = reader["Borrow_datetime"].ToString(),
                                        Return_datetime = reader["Return_datetime"].ToString(),
                                        Item_Id = reader["item_id"].ToString(),
                                        User_Id = reader["User_Id"].ToString(),
                                        CreateBy = reader["Create_By"].ToString(),
                                        CreateOn = reader["Create_On"].ToString()

                                    };
                                    BorrowRecords.Add(BorrowRecord);
                                }
                                log.Debug($"{sql}-[{i}]");
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            log.Error($"BorrowRecords Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                log.Error($"BorrowRecords Error: {ex.Message}");
            }
            return BorrowRecords;
        }
        #endregion

    }

    public class EventModel
    {
        public string Id { get; set; }
        public string Event_datetime { get; set; }
        public string Event_Type { get; set; }
        public string Event_Description { get; set; }
        public string User_Id { get; set; }
        public string CreateBy { get; set; }
        public string CreateOn { get; set; }

    }

    public class BorrowModel
    {
        public string Id { get; set; }
        public string User_Id { get; set; }
        public string Item_Id { get; set; }
        public string Return_datetime { get; set; }
        public string take_Datetime { get; set; }
        public string CreateBy { get; set; }
        public string CreateOn { get; set; }

    }
}
