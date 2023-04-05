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
    /// Логика взаимодействия для Journal.xaml
    /// </summary>
    public partial class Journal : Window
    {
        public Journal()
        {
            InitializeComponent();
            info.Content = MainWindow.myName+"\n"+ MainWindow.Status;
        }

        private void AddStudentClick(object sender, RoutedEventArgs e)
        {
            AddMenu add = new AddMenu();
            add.ShowDialog();
        }
    }
}
