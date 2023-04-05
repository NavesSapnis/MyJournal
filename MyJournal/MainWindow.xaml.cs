using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyJournal
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker _worker;
        public static string myName { get; set; }
        public static string Status { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.DoWork += worker_DoWork;
            _worker.ProgressChanged += worker_ProgressChanged;
            _worker.RunWorkerAsync();
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                bool isChecked = (bool)teacher.Dispatcher.Invoke((Func<bool>)(() => (bool)teacher.IsChecked));
                _worker.ReportProgress(isChecked ? 1 : 0);
                Thread.Sleep(100);
            }
        }
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            sing_up.Visibility = e.ProgressPercentage == 1 ? Visibility.Visible : Visibility.Collapsed;
        }
        public void NextWindow()
        {
            Journal journal = new Journal();
            Close();
            journal.Show();
        }
        private void LoginClick(object sender, RoutedEventArgs e)
        {
            if ((bool)teacher.IsChecked)
            {
                if(Sql.CheckTeacher(login.Text,password.Text))
                {
                    myName= login.Text;
                    Status = "Учитель";
                    NextWindow();
                }
                else
                {
                    Message.Alert("Такого учителя нет");
                }
            }
            else
            {
                if(Sql.CheckStudent(login.Text, password.Text))
                {
                    myName = login.Text;
                    Status = "Ученик";
                    NextWindow();
                }
                else
                {
                    Message.Alert("Такого ученика нет");
                }
            }
        }

        private void SingUpClick(object sender, RoutedEventArgs e)
        {
            if ((bool)teacher.IsChecked)
            {
                try
                {
                    Sql.AddTeacher(login.Text, password.Text);
                    Message.Alert("Учитель добавлен");
                }
                catch
                {
                    Message.Alert("Такой учитель уже есть");
                }
            }
            else
            {
                Message.Alert("Нельзя зарегистрироваться будучи учеником");
            }
        }
    }
}
