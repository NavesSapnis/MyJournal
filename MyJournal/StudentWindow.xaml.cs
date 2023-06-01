using MyJournal.Class;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public StudentWindow()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            var subjects = Sql.GetSubjectsForGroup(Sql.GetStudentGroup("Алекс"));
            DataTable dataTable = new DataTable();
            for(int i = 0;i< subjects.Count; i++)
            {
                dataTable.Columns.Add(subjects[i].ToString());
            }
            data.ItemsSource = dataTable.DefaultView;
        }
    }
}
