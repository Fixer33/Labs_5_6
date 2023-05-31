using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Shared;

namespace Server
{
    internal static class Database
    {
        private static SQLiteConnection _connection;

        public static void Initialize()
        {
            string connStr = "Data Source=database.db; Compress = True; ";
            _connection = new SQLiteConnection(connStr);
            try
            {
                _connection.Open();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Can't open connection to db!");
                Console.ResetColor();
                throw new Exception("No connection to db!");
            }
        }

        public static List<UserData> GetAllUsers()
        {
            List<UserData> users = new List<UserData>();
            SQLiteDataReader dataReader;
            SQLiteCommand comm;
            comm = _connection.CreateCommand();
            comm.CommandText = "SELECT * FROM Users;";

            dataReader = comm.ExecuteReader();
            while (dataReader.Read())
            {
                users.Add(new UserData(dataReader.GetString(0), dataReader.GetString(1)));
            }
            return users;
        }

        public static UserData GetUser(string id)
        {
            SQLiteDataReader dataReader;
            SQLiteCommand comm;
            comm = _connection.CreateCommand();

            string command = "SELECT * FROM Users where id == ";
            command += @"""";
            command += id;
            command += @""";";
            comm.CommandText = command;

            dataReader = comm.ExecuteReader();
            while (dataReader.Read())
            {
                return new UserData(dataReader.GetString(0), dataReader.GetString(1));
            }

            return null;
        }

        public static void AddUser(string id)
        {
            SQLiteCommand comm;
            comm = _connection.CreateCommand();
            comm.CommandText = $@"insert into Users values (""{id}"", ""{id}"");";
            comm.ExecuteNonQuery();
        }

        public static void UpdateUserName(string id, string newName)
        {
            SQLiteCommand comm;
            comm = _connection.CreateCommand();
            comm.CommandText = $@"UPDATE Users set name = ""{newName}"" where id == ""{id}"";";
            comm.ExecuteNonQuery();
        }

        public static bool CheckUserIdExistance(string id)
        {
            SQLiteDataReader dataReader;
            SQLiteCommand comm;
            comm = _connection.CreateCommand();

            comm.CommandText = $"SELECT COUNT(id) from Users where id == '{id}';";

            dataReader = comm.ExecuteReader();
            while (dataReader.Read())
            {
                return dataReader.GetInt32(0) > 0;
            }

            return false;
        }

        public static int GetFreeChatId()
        {
            SQLiteDataReader dataReader;
            SQLiteCommand comm;
            comm = _connection.CreateCommand();

            comm.CommandText = "select max(id) from Chats;";

            dataReader = comm.ExecuteReader();
            while (dataReader.Read())
            {
                return dataReader.GetInt32(0) + 1;
            }

            return -1;
        }

        public static void CreateChat(ChatData chat)
        {
            SQLiteCommand comm;
            comm = _connection.CreateCommand();
            comm.CommandText = $"insert into Chats values ({chat.Id}, '{chat.Serialize()}');";
            comm.ExecuteNonQuery();
        }

        public static List<ChatData> GetUserChats(string userId)
        {
            SQLiteDataReader dataReader;
            SQLiteCommand comm;
            comm = _connection.CreateCommand();

            comm.CommandText = $@"SELECT * from Chats where serializeddata like '%""{userId}""%';";

            List<ChatData> result = new List<ChatData>();
            dataReader = comm.ExecuteReader();
            while (dataReader.Read())
            {
                result.Add(ChatData.Deserialize(dataReader.GetString(1)));
            }

            return result;
        }

        public static void Close()
        {
            _connection.Close();
        }

        public struct UserRecord
        {
            public string Id;
            public string Name;
        }
    }
}
