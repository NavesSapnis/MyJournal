using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.Data.SqlClient;
using System.Data;

namespace MyJournal
{
    public class Sql
    {
        private Sql() { }
        public static string connectionString = "Data Source=mydatabase.db";
        
        public static void AddSubject(string SubjectName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("INSERT INTO Subjects (SubjectName) VALUES (@SubjectName)", connection))
                {
                    command.Parameters.AddWithValue("@SubjectName", SubjectName);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public static void AddGroup(string GroupName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("INSERT INTO Groups (GroupName) VALUES (@GroupName)", connection))
                {
                    command.Parameters.AddWithValue("@GroupName", GroupName);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public static DataTable LoadDataFromTable(string name)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM {name}";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);

                DataTable dataSource = new DataTable();

                adapter.Fill(dataSource);
                connection.Close();
                return dataSource;
            }
        }
        //public static void AddTeacher(string login, string password)
        //{
        //    using (var connection = new SQLiteConnection(connectionString))
        //    {
        //        connection.Open();
        //        using (var command = new SQLiteCommand("INSERT INTO Teachers (Login, Password) VALUES (@Login, @Password)", connection))
        //        {
        //            command.Parameters.AddWithValue("@Login", login);
        //            command.Parameters.AddWithValue("@Password", password);
        //            command.ExecuteNonQuery();
        //        }
        //        connection.Close();
        //    }
        //}
        //public static bool CheckStudent(string login, string password)
        //{
        //    bool userExists = false;
        //    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        //    {
        //        connection.Open();

        //        string query = "SELECT COUNT(*) FROM Students WHERE Login=@Login AND Password=@Password";
        //        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@Login", login);
        //            command.Parameters.AddWithValue("@Password", password);

        //            int count = Convert.ToInt32(command.ExecuteScalar());
        //            if (count > 0)
        //            {
        //                userExists = true;
        //            }
        //        }
        //        connection.Close();
        //    }
        //    return userExists;
        //}
        //public static bool CheckTeacher(string login, string password)
        //{
        //    bool userExists = false;
        //    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        //    {
        //        connection.Open();

        //        string query = "SELECT COUNT(*) FROM Teachers WHERE Login=@Login AND Password=@Password";
        //        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@Login", login);
        //            command.Parameters.AddWithValue("@Password", password);

        //            int count = Convert.ToInt32(command.ExecuteScalar());
        //            if (count > 0)
        //            {
        //                userExists = true;
        //            }
        //        }
        //        connection.Close();
        //    }
        //    return userExists;
        //}
        //public static void AddGroup(string name,string group)
        //{
        //    using (var connection = new SQLiteConnection(connectionString))
        //    {
        //        connection.Open();
        //        using (var command = new SQLiteCommand("INSERT INTO Teachers (Login) VALUES (@Login))", connection))
        //        {
        //            command.CommandText = "INSERT INTO Teachers WHERE Login = @Login";
        //            command.Parameters.AddWithValue("@Groups", group);
        //            command.ExecuteNonQuery();
        //        }
        //        connection.Close();
        //    }
        //}
    }
}
