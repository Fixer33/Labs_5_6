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
