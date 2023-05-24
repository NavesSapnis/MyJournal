using MyJournal.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyJournal
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public string tableName { get; set; }
        public void ClearSubjectName(){subjectName.Text = subjectName.Tag.ToString();}
        public void ClearGroupName(){groupName.Text = groupName.Tag.ToString();}
        public void ClearUserInfo() 
        { 
            name.Text = name.Tag.ToString();
            password.Text = password.Tag.ToString();
            groups.Text = groups.Tag.ToString();
        }
        public void RemoveText(object sender, EventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (instance.Text == instance.Tag.ToString())
                instance.Text = "";
        }
        public void AddText(object sender, EventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(instance.Text))
                instance.Text = instance.Tag.ToString();
        }
        public void RefreshTable()
        {
            var dataSource = Sql.LoadDataFromTable(tableName);
            data.ItemsSource = dataSource.DefaultView;
        }
        public void tableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBoxItem selectedItem = (ComboBoxItem)tableComboBox.SelectedItem;
                tableName = selectedItem.Content.ToString();
                RefreshTable();
            }
            catch { }
        }

        public Admin()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
        }

        public void AddGroup(object sender, RoutedEventArgs e)
        {
            var name = groupName.Text;
            try
            {
                if (!string.IsNullOrEmpty(name) && name != "Номер группы")
                {
                    Sql.AddGroup(name);
                    MessageBox.Show($"Группа \"{name}\" успешно добавлена!");
                }
                else
                {
                    MessageBox.Show("Необходимо ввести название группы");
                }
            }
            catch
            {
                MessageBox.Show("Такая группа уже есть");
            }
            ClearGroupName();
            RefreshTable();
        }
        public void AddTeacher(object sender, RoutedEventArgs e)
        {
            var name_ = name.Text;
            var password_ = password.Text;
            var group_ = groups.Text;
            try
            {
                if (!string.IsNullOrEmpty(name_) && (!string.IsNullOrEmpty(password_)))
                {
                    Sql.AddTeacher(name_, password_, group_);
                    MessageBox.Show("Учитель добавлен");
                }
            }
            catch
            {
                MessageBox.Show("Такой учитель уже есть");
            }
            ClearUserInfo();
            RefreshTable();
        }
        public void AddStudent(object sender, RoutedEventArgs e)
        {
            var name_ = name.Text;
            var password_ = password.Text;
            var group_ = groups.Text;
            try
            {
                if (!string.IsNullOrEmpty(name_) && (!string.IsNullOrEmpty(password_)))
                {
                    Sql.AddStudent(name_, password_, group_);
                    MessageBox.Show("Ученик добавлен");
                }
            }
            catch
            {
                MessageBox.Show("Такой ученик уже есть");
            }
            ClearUserInfo();
            RefreshTable();
        }
        public void AddSubject(object sender, RoutedEventArgs e)
        {
            var name = subjectName.Text;
            try
            {
                if (!string.IsNullOrEmpty(name) && name != "Название предмета")
                {
                    Sql.AddSubject(name);
                    MessageBox.Show($"Предмет \"{name}\" успешно добавлен!");
                }
                else
                {
                    MessageBox.Show("Необходимо ввести название предмета");
                }
            }
            catch
            {
                MessageBox.Show("Такой предмет уже есть");
            }
            ClearSubjectName();
            RefreshTable();
        }
        public void RemoveGroup(object sender, RoutedEventArgs e)
        {
            var name_ = groupName.Text;
            Sql.RemoveGroup(name_);
            ClearGroupName();
            RefreshTable();
        }
        public void RemoveTeacher(object sender, RoutedEventArgs e)
        {
            var name_ = name.Text;
            Sql.RemoveTeacher(name_);
            ClearUserInfo();
            RefreshTable();
        }
        public void RemoveStudent(object sender, RoutedEventArgs e)
        {
            var name_ = name.Text;
            Sql.RemoveStudent(name_);
            ClearUserInfo();
            RefreshTable();
        }
        public void RemoveSubject(object sender, RoutedEventArgs e)
        {
            var name_ = subjectName.Text;
            Sql.RemoveSubject(name_);
            ClearSubjectName();
            RefreshTable();
        }
        public void Save(object sender, RoutedEventArgs e)
        {
            
            if (isEditing)
            {
                var table = GetDataTableFromDataGrid();
                switch (tableName)
                {
                    case "Groups":

                        break;
                    case "GroupSubject":

                        break;
                    case "Students":

                        break; 
                    case "Subjects":
                        Sql.SaveSubjects(table);
                        break;
                    case "Teachers":
                        Sql.SaveTeachers(table);
                        break;
                    case "TeachersGroups":

                        break;
                }
                
                MessageBox.Show("Успешно добавлено");
            }
            else
            {
                MessageBox.Show("Изменений не было");
            }
            RefreshTable();
        }
        public bool isEditing = false;
        public void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            isEditing = true;
        }
        public void RemoveEmptyRows(DataTable dataTable)
        {
            for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = dataTable.Rows[i];

                bool isEmpty = true;
                foreach (var item in row.ItemArray)
                {
                    if (!string.IsNullOrEmpty(item?.ToString()))
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty)
                {
                    dataTable.Rows.Remove(row);
                }
            }
        }
        public DataTable GetDataTableFromDataGrid()
        {

            DataTable dataTable = new DataTable();
            foreach (DataGridColumn column in data.Columns)
            {
                dataTable.Columns.Add(column.Header.ToString());
            }

            foreach (var item in data.Items)
            {
                DataRow newRow = dataTable.NewRow();
                
                foreach (DataGridColumn column in data.Columns)
                {
                    TextBlock textBlock = column.GetCellContent(item) as TextBlock;
                    if (textBlock != null)
                    {
                        newRow[column.Header.ToString()] = textBlock.Text;
                    }
                }
                dataTable.Rows.Add(newRow);
            }
            RemoveEmptyRows(dataTable);
            return dataTable;
        }
    }
}
