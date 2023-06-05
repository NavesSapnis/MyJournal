using MyJournal.Class;
using MyJournal.MarkHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace MyJournal
{
    /// <summary>
    /// Логика взаимодействия для TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        //public new string Name = MainWindow.Name;
        public new string Name = "Алексий В.О.";
        public TeacherWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            LoadGroups();
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
            Admin.RemoveEmptyRows(dataTable);
            return dataTable;
            //Рабочие костыли ура
        }
        public void LoadGroups()
        {
            var groups = Sql.GetGroupsOfTeacher(Name);
            Group.DisplayMemberPath = "GroupName";
            Group.SelectedValuePath = "GroupId";
            Group.ItemsSource = groups.DefaultView;
        }
        public void LoadSubjects(int GroupId)
        {
            var subjectsOfThisGroup = Sql.GetSubjectsForGroupDataTable(GroupId);
            Subject.DisplayMemberPath = "SubjectName";
            Subject.SelectedValuePath = "SubjectId";
            Subject.ItemsSource = subjectsOfThisGroup.DefaultView;
        }
        public void LoadStudentSubjectMarks(List<string> students)
        {
            DataTable dataTable = new DataTable();
            for (int i = 0; i < students.Count; i++)
            {
                dataTable.Columns.Add(students[i].ToString());
            }
            for (int i = 0; i < students.Count; i++)
            {
                int rowIndex = 0;
                var grades = Sql.GetStudentSubjectGrades(students[i], Sql.GetSubjectNameById(Convert.ToInt32(Subject.SelectedValue)));
                foreach (var grade in grades)
                {
                    dataTable.Rows.Add();
                    dataTable.Rows[rowIndex].SetField(students[i], grade);
                    rowIndex++;
                } 
            }
            Admin.RemoveEmptyRows(dataTable);
            data.ItemsSource = dataTable.DefaultView;
        }
        private void Group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSubjects(Convert.ToInt32(Group.SelectedValue));
        }

        private void Subject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadStudentSubjectMarks(Sql.GetStudentsOfGroup(Convert.ToInt32(Group.SelectedValue)));
        }
        public void Save()
        {
            int studentIndex = 0;
            var dataTable = GetDataTableFromDataGrid();
            for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                string name = data.Columns[studentIndex].Header.ToString();
                Sql.RemoveMarks(Sql.GetStudentIdByName(name), Convert.ToInt32(Subject.SelectedValue));
                for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                {
                    object cellValue = dataTable.Rows[rowIndex][columnIndex];
                    try
                    {
                        int grade = Convert.ToInt32(cellValue);
                        Sql.AddMark(Sql.GetStudentIdByName(name), Convert.ToInt32(Subject.SelectedValue),grade);
                    }
                    catch
                    {

                    }
                }
                studentIndex++;
            }
        }
        private void SaveAction(object sender, RoutedEventArgs e)
        {
            Save();
        }
    }
}
