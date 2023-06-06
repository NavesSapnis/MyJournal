using MyJournal.Export;
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
        public new string Name = "Иванов_И_П";
        public TeacherWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            LoadGroups();
            LoadStudentsComboBox();
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
        public void LoadStudentsComboBox()
        {
            int groupId = Sql.GetTeacherGroupId(Sql.GetTeacherIdByName(Name));
            var students = Sql.GetStudentsOfGroup(groupId);
            Student.ItemsSource = students;
            Student.SelectedIndex = 0;
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
                    dataTable.Rows[rowIndex].SetField(students[i].ToString(), grade);
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

        private void Refresh(object sender, RoutedEventArgs e)
        {
            LoadStudentSubjectMarks(Sql.GetStudentsOfGroup(Convert.ToInt32(Group.SelectedValue)));
        }

        private void StudentsSkill(object sender, RoutedEventArgs e)
        {
            var studentName = Student.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(studentName))
            {
                var table = LoadStudent(studentName);
                DataTableToWordExporter.FileSave(studentName, table, "C:\\Users\\veih\\Source\\Repos\\NavesSapnis\\MyJournal\\MyJournal\\Exported\\exp1.docx");
                MessageBox.Show("Эскпортированно");
            }
        }
        public static DataTable LoadStudent(string Name)
        {
            var subjects = Sql.GetSubjectsForGroup(Sql.GetStudentGroup(Name));
            DataTable dataTable = new DataTable();
            for (int i = 0; i < subjects.Count; i++)
            {
                dataTable.Columns.Add(subjects[i].ToString());
            }
            for (int i = 0; i < subjects.Count; i++)
            {
                var rowIndex = 0;
                var grades = Sql.GetStudentSubjectGrades(Sql.GetStudentIdByName(Name), Sql.GetSubjectIdByName(subjects[i]));
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
            return dataTable;
        }
        private void Student_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GroupSkill_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var table = GetDataTableFromDataGrid();
                var group = Sql.GetGroupNameById(Convert.ToInt32(Group.SelectedValue));
                var subject = Sql.GetSubjectNameById(Convert.ToInt32(Subject.SelectedValue)); var all = group + " " + subject;
                DataTableToWordExporter.FileSave(all, table, "C:\\Users\\veih\\Source\\Repos\\NavesSapnis\\MyJournal\\MyJournal\\Exported\\exp2.docx");
                MessageBox.Show("Эскпортированно");

            }
            catch { MessageBox.Show("Выберите группу и ее предмет"); }
        }

        private void MyGroupSkill_Click(object sender, RoutedEventArgs e)
        {
            List<DataTable> dataTables = new List<DataTable>();
            var group = Sql.GetTeacherGroupId(Sql.GetTeacherIdByName(Name));
            var groupName = Sql.GetGroupNameById(group);
            var students = Sql.GetStudentsOfGroup(group);
            foreach (var student in students)
            {
                var table = LoadStudent(student);
                dataTables.Add(table);
            }
            DataTableToWordExporter.FileSave(groupName, students, dataTables, "C:\\Users\\veih\\Source\\Repos\\NavesSapnis\\MyJournal\\MyJournal\\Exported\\exp3.docx");
            MessageBox.Show("Эскпортированно");
        }
    }
}
