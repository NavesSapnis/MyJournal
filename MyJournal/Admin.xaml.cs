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
        public bool isEditing = false;
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
            LoadData();
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
            ClearComboBoxes();
            LoadData();
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
        }//Добавление группы в бд через text box
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
        }//Добавление учителя в бд через text box
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
        }//Добавление студента в бд через text box
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
        }//Добавление предмета в бд через text box
        public void RemoveGroup(object sender, RoutedEventArgs e)
        {
            var name_ = groupName.Text;
            Sql.RemoveGroup(name_);
            ClearGroupName();
            RefreshTable();
        }//Удаление группы из бд через text box
        public void RemoveTeacher(object sender, RoutedEventArgs e)
        {
            var name_ = name.Text;
            Sql.RemoveTeacher(name_);
            ClearUserInfo();
            RefreshTable();
        }//Удаление учителя из бд через text box
        public void RemoveStudent(object sender, RoutedEventArgs e)
        {
            var name_ = name.Text;
            Sql.RemoveStudent(name_);
            ClearUserInfo();
            RefreshTable();
        }//Удаление студента из бд через text box
        public void RemoveSubject(object sender, RoutedEventArgs e)
        {
            var name_ = subjectName.Text;
            Sql.RemoveSubject(name_);
            ClearSubjectName();
            RefreshTable();
        }//Удаление предмета из бд через text box
        public void Save(object sender, RoutedEventArgs e)
        {
            
            if (isEditing)
            {
                try
                {
                    var table = GetDataTableFromDataGrid();
                    Sql.Save(table, tableName);
                    RefreshTable();
                }
                catch { MessageBox.Show("Ошибка при сохранении"); }

            }

        }//Сохрание изменений в DataGrid TODO УДАЛЕНИЕ
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
                //TODO Попробовать проходиться вот так
                if (isEmpty)
                {
                    dataTable.Rows.Remove(row);
                }
            }
        }//Удаление пустых строк из дататейбла
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
            //Рабочие костыли ура
        }
        public void ClearComboBoxes()
        {
            SelectSubject.Items.Clear();
            SelectTeacher.Items.Clear();
            SelectGroup.Items.Clear();
            SelectGroupTeacher.Items.Clear ();
        }
        public void LoadData() 
        {
            var subjects = Sql.LoadDataFromTable("Subjects");
            SelectSubject.DisplayMemberPath = "SubjectName";
            SelectSubject.SelectedValuePath = "SubjectId";
            SelectSubject.ItemsSource = subjects.DefaultView;
            
            var teachers = Sql.LoadDataFromTable("Teachers");
            SelectTeacher.DisplayMemberPath = "Name";
            SelectTeacher.SelectedValuePath = "Id";
            SelectTeacher.ItemsSource = teachers.DefaultView;

            var groups = Sql.LoadDataFromTable("Groups");
            SelectGroup.DisplayMemberPath = "GroupName"; SelectGroupTeacher.DisplayMemberPath = "GroupName";
            SelectGroup.SelectedValuePath = "GroupId";SelectGroupTeacher.SelectedValuePath = "GroupId";
            SelectGroup.ItemsSource = groups.DefaultView; SelectGroupTeacher.ItemsSource = groups.DefaultView;

            SelectSubject.SelectedIndex = 0;SelectTeacher.SelectedIndex = 0;SelectGroup.SelectedIndex = 0; SelectGroupTeacher.SelectedIndex = 0;
        }
        public void AddGroups(object sender, RoutedEventArgs e)
        {
            Sql.AddGroupSubject(SelectSubject.SelectedValue.ToString(),SelectGroup.SelectedValue.ToString()); RefreshTable();
        }
        public void RemoveGroups(object sender, RoutedEventArgs e)
        {
            Sql.RemoveGroupSubject(SelectSubject.SelectedValue.ToString(),SelectGroup.SelectedValue.ToString()); RefreshTable();
        }
        private void AddGroupsTeacher(object sender, RoutedEventArgs e)
        {
            Sql.AddTeachersGroups(SelectTeacher.SelectedValue.ToString(),SelectGroupTeacher.SelectedValue.ToString()); RefreshTable();
        }
        private void RemoveGroupsTeacher(object sender, RoutedEventArgs e)
        {
            Sql.RemoveTeachersGroups(SelectTeacher.SelectedValue.ToString(), SelectGroupTeacher.SelectedValue.ToString()); RefreshTable();
        }
        public void DeleteTable(object sender, RoutedEventArgs e)
        {
            try
            {
                Sql.DeleteAll(tableName);
                MessageBox.Show($"Данные из таблицы {tableName} удалены");
                RefreshTable();
            }
            catch { }
        }
    }
}
