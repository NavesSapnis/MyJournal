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
        public new string Name = "Иванов И.П.";
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
            var groups = Sql.GetGroupsOfTeacher(Sql.GetTeacherIdByName(Name));
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
                var grades = Sql.GetStudentSubjectGrades(Sql.GetStudentIdByName(students[i]), Convert.ToInt32(Subject.SelectedValue));
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
        public void Save(object sender, RoutedEventArgs e)
        {
            var dataTable = GetDataTableFromDataGrid();
            foreach (DataColumn column in dataTable.Columns)
            {
                string name = column.ColumnName;
                int studentId = Sql.GetStudentIdByName(name);
                int subjectId = Convert.ToInt32(Subject.SelectedValue);
                Sql.RemoveMarks(studentId, subjectId);
                foreach (DataRow row in dataTable.Rows)
                {
                    object cellValue = row[name];
                    try
                    {
                        if (!string.IsNullOrEmpty(cellValue.ToString()))
                        {
                            int grade = Convert.ToInt32(cellValue);
                            if (grade > 0 && grade < 11)
                            {
                                Sql.AddMark(studentId, subjectId, grade);
                            }
                            else
                            {
                                MessageBox.Show("Символы и оценки которые не входили в диапазон 0<x>11 удалены");
                            }
                        }
                    }
                    catch { }
                }
            }
        }
    }
}
