using MyJournal.Class;
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
        public void LoadGroups()
        {
            var groups = Sql.GetGroupsOfTeacher(Name);
            Group.DisplayMemberPath = "GroupName";
            Group.SelectedValuePath = "GroupId";
            Group.ItemsSource = groups.DefaultView;
        }
        public void LoadSubjects(int GroupId)
        {
            Subject.Items.Clear();
            var subjectsOfThisGroup = Sql.GetSubjectsForGroupDataTable(GroupId);
            Subject.DisplayMemberPath = "SubjectName";
            Subject.SelectedValuePath = "SubjectId";
            Subject.ItemsSource = subjectsOfThisGroup.DefaultView;
        }
        public void LoadStudents(List<string> students)
        {
            DataTable dataTable = new DataTable();
            for (int i = 0; i < students.Count; i++)
            {
                dataTable.Columns.Add(students[i].ToString());
            }
            data.ItemsSource = dataTable.DefaultView;
        }
        private void Group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSubjects(Convert.ToInt32(Group.SelectedValue));
            LoadStudents(Sql.GetStudentsOfGroup(Convert.ToInt32(Group.SelectedValue)));
        }
    }
}
