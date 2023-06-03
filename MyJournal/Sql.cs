using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using System.Windows.Markup;
using MyJournal.Class;
using System.Windows.Controls;
using System.Text.RegularExpressions;

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
        public static void RemoveSubject(string SubjectName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand($"DELETE FROM Subjects WHERE SubjectName = '{SubjectName}'", connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Предмет успешно удален.");
                    }
                    else
                    {
                        MessageBox.Show($"Предмет не найден.");
                    }
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
        public static void RemoveGroup(string GroupName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand($"DELETE FROM Groups WHERE GroupName = '{GroupName}'", connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Группа успешно удалена.");
                    }
                    else
                    {
                        MessageBox.Show($"Группа не найдена.");
                    }
                }
                connection.Close();
            }
        }
        public static void AddTeacher(string Name, string Password, string GroupName)
        {
            var MainGroup = GetIdGroupByName(GroupName);
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("INSERT INTO Teachers (Name, Password, MainGroup) VALUES (@Name, @Password, @MainGroup)", connection))
                {
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@MainGroup", MainGroup);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public static void RemoveTeacher(string Name)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand($"DELETE FROM Teachers WHERE Name = '{Name}';", connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Учитель успешно удален.");
                    }
                    else
                    {
                        MessageBox.Show($"Учитель не найден.");
                    }
                }
                connection.Close();
            }
        }
        public static void AddStudent(string Name, string Password, string MainGroup)
        {
            var GroupId = GetIdGroupByName(MainGroup);
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("INSERT INTO Students (Name, Password, GroupId) VALUES (@Name, @Password, @GroupId)", connection))
                {
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@GroupId", GroupId);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public static void RemoveStudent(string Name)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand($"DELETE FROM Students WHERE Name = '{Name}'", connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Ученик успешно удален.");
                    }
                    else
                    {
                        MessageBox.Show($"Ученик не найден.");
                    }
                }
                connection.Close();
            }
        }
        public static void AddGroupSubject(string SubjectId, string GroupId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var test = new SQLiteCommand($"SELECT COUNT(*) FROM GroupSubject WHERE SubjectId = {SubjectId} AND GroupId = {SubjectId}", connection))
                {
                    int count = Convert.ToInt32(test.ExecuteScalar());
                    if (count == 0)
                    {
                        using (var command = new SQLiteCommand("INSERT INTO GroupSubject (SubjectId, GroupId) VALUES (@SubjectId, @GroupId)", connection))
                        {
                            command.Parameters.AddWithValue("@SubjectId", SubjectId);
                            command.Parameters.AddWithValue("@GroupId", GroupId);
                            command.ExecuteNonQuery();
                        }
                    }
                    else { }
                    connection.Close();
                }
            }
        }
        public static void RemoveGroupSubject(string SubjectId, string GroupId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand($"DELETE FROM GroupSubject WHERE SubjectId = {SubjectId} AND GroupId = {GroupId}", connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Предмет у группы успешно удален.");
                    }
                    else
                    {
                        MessageBox.Show($"Удаление не успешно");
                    }
                }
                connection.Close();
            }
        }
        public static void AddTeachersGroups(string TeacherId, string GroupId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var test = new SQLiteCommand($"SELECT COUNT(*) FROM TeachersGroups WHERE TeacherId = {TeacherId} AND GroupId = {GroupId}", connection))
                {
                    int count = Convert.ToInt32(test.ExecuteScalar());
                    if (count == 0)
                    {
                        using (var command = new SQLiteCommand("INSERT INTO TeachersGroups (TeacherId, GroupId) VALUES (@TeacherId, @GroupId)", connection))
                        {
                            command.Parameters.AddWithValue("@TeacherId", TeacherId);
                            command.Parameters.AddWithValue("@GroupId", GroupId);
                            command.ExecuteNonQuery();
                        }
                    }
                    else { }
                    connection.Close();
                }
            }
        }
        public static void RemoveTeachersGroups(string TeacherId, string GroupId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand($"DELETE FROM TeachersGroups WHERE TeacherId = {TeacherId} AND GroupId = {GroupId}", connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Группа у учителя успешна удалена.");
                    }
                    else
                    {
                        MessageBox.Show($"Удаление не успешно");
                    }
                }
                connection.Close();
            }
        }
        public static void Save(DataTable dataTable,string table)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "";
                switch (table)
                {
                    case "Groups":
                        insertQuery = $"INSERT INTO Groups (GroupName) VALUES (@GroupName)";
                        using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                        {
                            command.Parameters.Add("@GroupName", DbType.String);

                            foreach (DataRow row in dataTable.Rows)
                            {
                                try
                                {
                                    command.Parameters["@GroupName"].Value = row["GroupName"];
                                    command.ExecuteNonQuery();
                                }
                                catch { }
                            }
                        }
                        break;
                    case "GroupSubject":
                        insertQuery = $"INSERT INTO GroupSubject (SubjectId, GroupId) VALUES (@SubjectId, @GroupId)";
                        using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                        {
                            command.Parameters.Add("@SubjectId", DbType.Int32);
                            command.Parameters.Add("@GroupId", DbType.Int32);

                            foreach (DataRow row in dataTable.Rows)
                            {
                                try
                                {
                                    command.Parameters["@SubjectId"].Value = row["SubjectId"];
                                    command.Parameters["@GroupId"].Value = row["GroupId"];
                                    command.ExecuteNonQuery();
                                }
                                catch { }
                            }
                        }
                        break;
                    case "Students":
                        insertQuery = $"INSERT INTO Students (Name, Password, GroupId) VALUES (@Name, @Password, @GroupId)";
                        using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                        {
                            command.Parameters.Add("@Name", DbType.String);
                            command.Parameters.Add("@Password", DbType.String);
                            command.Parameters.Add("@GroupId", DbType.Int32);

                            foreach (DataRow row in dataTable.Rows)
                            {
                                try
                                {
                                    command.Parameters["@Name"].Value = row["Name"];
                                    command.Parameters["@Password"].Value = row["Password"];
                                    command.Parameters["@GroupId"].Value = row["GroupId"];
                                    command.ExecuteNonQuery();
                                }
                                catch { }
                            }
                        }
                        break;
                    case "Subjects":
                        insertQuery = $"INSERT INTO Subjects (SubjectName) VALUES (@SubjectName)";
                        using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                        {
                            command.Parameters.Add("@SubjectName", DbType.String);

                            foreach (DataRow row in dataTable.Rows)
                            {
                                try
                                {
                                    command.Parameters["@SubjectName"].Value = row["SubjectName"];
                                    command.ExecuteNonQuery();
                                }
                                catch { }
                            }
                        }
                        break;
                    case "Teachers":
                        insertQuery = $"INSERT INTO Teachers (Name, Password, MainGroup) VALUES (@Name, @Password, @MainGroup)";
                        using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                        {
                            command.Parameters.Add("@Name", DbType.String);
                            command.Parameters.Add("@Password", DbType.String);
                            command.Parameters.Add("@MainGroup", DbType.Int32);

                            foreach (DataRow row in dataTable.Rows)
                            {
                                try
                                {
                                    command.Parameters["@Name"].Value = row["Name"];
                                    command.Parameters["@Password"].Value = row["Password"];
                                    command.Parameters["@MainGroup"].Value = row["MainGroup"];
                                    command.ExecuteNonQuery();
                                }
                                catch { }
                            }
                        }
                        break;
                    case "TeachersGroups":
                        insertQuery = $"INSERT INTO TeachersGroups (TeacherId, GroupId) VALUES (@TeacherId, @GroupId)";
                        using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                        {
                            command.Parameters.Add("@TeacherId", DbType.Int32);
                            command.Parameters.Add("@GroupId", DbType.Int32);

                            foreach (DataRow row in dataTable.Rows)
                            {
                                try
                                {
                                    command.Parameters["@TeacherId"].Value = row["TeacherId"];
                                    command.Parameters["@GroupId"].Value = row["GroupId"];
                                    command.ExecuteNonQuery();
                                }
                                catch { }
                            }
                        }
                        break;
                }
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
        public static List<string> GetStudentsOfGroup(int GroupId)
        {
            List<string> students = new List<string>();
            string query = "SELECT * FROM Students WHERE GroupId = @GroupId";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GroupId", GroupId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string student = reader.GetString(1);
                            students.Add(student);
                        }
                    }
                }
                connection.Close();
            }

            return students;

        }
        public static DataTable GetSubjectsForGroupDataTable(int GroupId)
        {
            string query = "SELECT Subjects.SubjectId, Subjects.SubjectName FROM GroupSubject JOIN Subjects ON GroupSubject.SubjectId = Subjects.SubjectId WHERE GroupSubject.GroupId = @GroupId";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@GroupId", GroupId);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                connection.Close();
                return dataTable;
            }
        }
        public static string GetGroupNameById(int Id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = $"SELECT GroupName FROM Groups WHERE GroupId = {Id}";
                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        string groupName = result.ToString();
                        return groupName;
                    }
                    else
                    {
                        return "Кураторской группы нет";
                    }

                }
            }
        }
        public static int GetTeacherGroupId(string Name)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = $"SELECT MainGroup FROM Teachers WHERE Name = '{Name}';";
                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        int groupId = Convert.ToInt32(result);
                        return groupId;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
        }
        public static DataTable GetGroupsOfTeacher(string Name)
        {
            var mainGroup = GetTeacherGroupId(Name);
            string query = "SELECT Groups.GroupId, Groups.GroupName FROM Groups JOIN Teachers ON Groups.GroupId = Teachers.Id WHERE Teachers.Name = @Name";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@Name", Name);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                int groupId = GetTeacherGroupId(Name);
                string groupName = GetGroupNameById(groupId);
                dataTable.Rows.Add(groupId,groupName);
                connection.Close();
                return dataTable;
            }
        }
        public static void DeleteAll(string table)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = $"DELETE FROM {table}";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public static int GetIdGroupByName(string groupName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = $"SELECT GroupId FROM Groups WHERE GroupName = '{groupName}';";

                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        int groupId = Convert.ToInt32(result);
                        return groupId;
                    }
                    else
                    {
                        Console.WriteLine($"Группа с названием '{groupName}' не найдена, добавьте ее.");
                        return 0;
                    }
                    
                }
            }
        }
        public static string GetSubjectNameById(int Id) 
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = $"SELECT SubjectName FROM Subjects WHERE SubjectId = {Id};";
                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    object result = command.ExecuteScalar();
                    string subjectName = result.ToString();
                    return subjectName;
                }
            }
        }
        public static bool AdminValidation(string Name, string Password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (var test = new SQLiteCommand($"SELECT COUNT(*) FROM Admin WHERE Name = \"{Name}\" AND Password = \"{Password}\"", connection))
                    {
                        int count = Convert.ToInt32(test.ExecuteScalar());
                        connection.Close();
                        if (count >= 1) { return true; }
                        else { return false; }
                    }

                }
                catch { return false; }
            }
        }
        public static bool LoginValidationTeacher(string Name, string Password)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var test = new SQLiteCommand($"SELECT COUNT(*) FROM Teachers WHERE Name = \"{Name}\" AND Password = \"{Password}\"", connection))
                    {
                        int count = Convert.ToInt32(test.ExecuteScalar());
                        connection.Close();
                        if (count == 1) { return true; }
                        else { return false; }
                    }
                }
            }
            catch { return false; }
        }
        public static bool LoginValidationStudent(string Name, string Password)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var test = new SQLiteCommand($"SELECT COUNT(*) FROM Students WHERE Name = \"{Name}\" AND Password = \"{Password}\"", connection))
                    {
                        int count = Convert.ToInt32(test.ExecuteScalar());
                        connection.Close();
                        if (count == 1) { return true; }
                        else { return false; }
                    }
                }
            }
            catch { return false; }
        }
        public static int GetStudentGroup(string studentName)
        {
            int groupId = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"
            SELECT g.GroupId
            FROM Students s
            JOIN Groups g ON s.GroupId = g.GroupId
            WHERE s.Name = @StudentName";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentName", studentName);

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        groupId = Convert.ToInt32(result);
                    }
                }
                connection.Close();
            }

            return groupId;
        }
        public static List<string> GetSubjectsForGroup(int groupId)
        {
            List<string> subjects = new List<string>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"
            SELECT s.SubjectId, s.SubjectName
            FROM GroupSubject gs
            JOIN Subjects s ON gs.SubjectId = s.SubjectId
            WHERE gs.GroupId = @GroupId";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GroupId", groupId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string subjectName = reader.GetString(1);
                            subjects.Add(subjectName);
                        }
                    }
                }
                connection.Close();
            }

            return subjects;
        }
        public static List<int> GetStudentSubjectGrades(string Name, string SubjectName)
        {
            List<int> grades = new List<int>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"
            SELECT m.Grade
            FROM Marks m
            JOIN Students s ON m.StudentId = s.Id
            JOIN Subjects subj ON m.SubjectId = subj.SubjectId
            WHERE s.Name = @StudentName
            AND subj.SubjectName = @SubjectName";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentName", Name);
                    command.Parameters.AddWithValue("@SubjectName", SubjectName);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int grade = Convert.ToInt32(reader["Grade"]);
                            grades.Add(grade);
                        }
                    }
                }
                connection.Close();
            }
            return grades;
        }
    }
}
