using MyJournal.MarkHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        public new string Name = MainWindow.Name;
        //public new string Name = "Сидорова С.И.";
        public StudentWindow()
        {
            InitializeComponent();
            LoadData();
            ResizeMode = ResizeMode.NoResize;
        }
        public void LoadData()
        {
            hello.Content = Hello();
            var subjects = Sql.GetSubjectsForGroup(Sql.GetStudentGroup(Name));
            DataTable dataTable = new DataTable();
            for(int i = 0;i< subjects.Count; i++)
            {
                dataTable.Columns.Add(subjects[i].ToString());
            } 
            for (int i = 0; i < subjects.Count; i++)
            {
                var rowIndex = 0;
                var grades = Sql.GetStudentSubjectGrades(Sql.GetStudentIdByName(Name),Sql.GetSubjectIdByName(subjects[i])); 
                foreach (var grade in grades)
                {
                    dataTable.Rows.Add();
                    dataTable.Rows[rowIndex].SetField(subjects[i], grade);
                    rowIndex++;
                }
                dataTable.Rows.Add();
                var srb = MarksHelper.GetAverage(grades.Sum(), grades.Count());
                if (double.IsNaN(srb))
                {
                    dataTable.Rows[rowIndex].SetField(subjects[i], "не атест.");
                }
                else
                {
                    dataTable.Rows[rowIndex].SetField(subjects[i], "Ср. балл: " + srb);
                }

            }
            Admin.RemoveEmptyRows(dataTable);
            data.ItemsSource = dataTable.DefaultView;
        }
        
        public string Hello()
        {
            return $"Приветствуем, {Name}";
        }
    }
}
