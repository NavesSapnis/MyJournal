using System;
using System.Collections.Generic;
using System.Data;
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
        public void tableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBoxItem selectedItem = (ComboBoxItem)tableComboBox.SelectedItem;
                string tableName = selectedItem.Content.ToString();
                var dataSource = Sql.LoadDataFromTable(tableName);
                data.ItemsSource = dataSource.DefaultView;
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
        }
        public void AddTeacher(object sender, RoutedEventArgs e)
        {
            var name_ = name.Text;
        }
        public void AddStudent(object sender, RoutedEventArgs e)
        {

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
        }
        public void RemoveGroup(object sender, RoutedEventArgs e)
        {

        }
        public void RemoveTeacher(object sender, RoutedEventArgs e)
        {

        }
        public void RemoveStudent(object sender, RoutedEventArgs e)
        {

        }
        public void RemoveSubject(object sender, RoutedEventArgs e)
        {

        }
    }
}
