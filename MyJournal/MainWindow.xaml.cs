using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MyJournal
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public new static string Name { get; set; }
        public static string Password { get; set; }
        public void EnterAdmin()
        {
            Admin admin = new Admin();
            Close();
            admin.Show();
        }
        public void EnterStudent()
        {
            StudentWindow student = new StudentWindow();
            Close();
            student.Show();
        }
        public void EnterTeacher()
        {
            TeacherWindow teacher = new TeacherWindow();
            Close();
            teacher.Show();
        }
        private void Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Next();
            }
        }
        public bool TeacherBox()
        {
            if ((bool)checkTeacher.IsChecked)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Next()
        {
            Name = name.Text;
            Password = password.Text;
            if (Sql.AdminValidation(Name, Password))
            {
                EnterAdmin();
            }
            else if (Sql.LoginValidationTeacher(Name, Password) && TeacherBox())
            {
                EnterTeacher();
            }
            else if (Sql.LoginValidationStudent(Name, Password))
            {
                EnterStudent();
            }
            else
            {
                error.Content = "Ошибка авторизации";
            }
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
        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
        }

        private void NextWindow(object sender, RoutedEventArgs e)
        {
            Next();
        }
    }
}
