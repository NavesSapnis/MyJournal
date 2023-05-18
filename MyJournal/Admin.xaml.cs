using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Admin()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
        }

        public void AddGroup(object sender, RoutedEventArgs e)
        {

        }

        public void AddTeacher(object sender, RoutedEventArgs e)
        {

        }
        public void AddStudent(object sender, RoutedEventArgs e)
        {

        }
        public void AddSubject(object sender, RoutedEventArgs e)
        {
            string subjectName = MessageBox.Show("Введите название предмета", "Добавить предмет", MessageBoxButton.OKCancel).ToString();


        if (!string.IsNullOrEmpty(subjectName))
        {
            
            MessageBox.Show($"Предмет \"{subjectName}\" успешно добавлен!");
        }
        else
        {
            MessageBox.Show("Необходимо ввести название предмета");
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
