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
    /// Логика взаимодействия для AddMenu.xaml
    /// </summary>
    public partial class AddMenu : Window
    {
        public AddMenu()
        {
            InitializeComponent();
        }

        private void SingUpClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Sql.AddStudent(login.Text, password.Text,group.Text,lessons.Text);
                Sql.AddGroup(MainWindow.myName,group.Text);
                Message.Alert("Ученик добавлен");
                Close();
            }
            catch
            {
                Message.Alert("Такой ученик уже есть");
            }
        }
    }
}
