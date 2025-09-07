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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp15
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StudentDbContext _ctx;

        public MainWindow()
        {
            InitializeComponent();
            _ctx = new StudentDbContext(); // предполагается: EF6 контекст с конструктором по умолчанию
            LoadAll();
        }

        private void LoadAll()
        {
            LoadStudents();
            LoadGrades();
            LoadAttendance();
        }

        private void LoadStudents()
        {
            var list = _ctx.Students.OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList();
            StudentsGrid.ItemsSource = list;
        }

        private void LoadGrades()
        {
            // Загрузим оценки и соберем имя студента для отображения
            var list = _ctx.Grades
                .Join(_ctx.Students,
                      g => g.StudentId,
                      s => s.Id,
                      (g, s) => new
                      {
                          g.Id,
                          g.StudentId,
                          StudentFullName = s.LastName + " " + s.FirstName,
                          g.Subject,
                          g.Score,
                          g.GradeDate
                      })
                .OrderByDescending(x => x.GradeDate)
                .ToList();
            GradesGrid.ItemsSource = list;
        }

        private void LoadAttendance()
        {
            var list = _ctx.Attendance
                .Join(_ctx.Students,
                      a => a.StudentId,
                      s => s.Id,
                      (a, s) => new
                      {
                          a.Id,
                          a.StudentId,
                          StudentFullName = s.LastName + " " + s.FirstName,
                          a.AttendanceDate,
                          a.IsPresent,
                          a.Note
                      })
                .OrderByDescending(x => x.AttendanceDate)
                .ToList();
            AttendanceGrid.ItemsSource = list;
        }

        #region Students buttons
        private void BtnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new StudentDialog();
            if (dlg.ShowDialog() == true)
            {
                var student = dlg.Student;
                _ctx.Students.Add(student);
                _ctx.SaveChanges();
                LoadStudents();
            }
        }

        private void BtnEditStudent_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsGrid.SelectedItem is null) return;
            dynamic row = StudentsGrid.SelectedItem;
            int id = (int)row.Id;
            var student = _ctx.Students.Find(id);
            if (student == null) return;

            var dlg = new StudentDialog(student);
            if (dlg.ShowDialog() == true)
            {
                _ctx.SaveChanges();
                LoadAll();
            }
        }

        private void BtnDeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsGrid.SelectedItem is null) return;
            dynamic row = StudentsGrid.SelectedItem;
            int id = (int)row.Id;
            var student = _ctx.Students.Find(id);
            if (student == null) return;
            if (MessageBox.Show($"Удалить студента {student.LastName} {student.FirstName}?", "Подтвердите", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _ctx.Students.Remove(student);
                _ctx.SaveChanges();
                LoadAll();
            }
        }
        #endregion

        #region Grades buttons
        private void BtnAddGrade_Click(object sender, RoutedEventArgs e)
        {
            var students = _ctx.Students.OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList();
            var dlg = new GradeDialog(students);
            if (dlg.ShowDialog() == true)
            {
                _ctx.Grades.Add(dlg.Grade);
                _ctx.SaveChanges();
                LoadGrades();
            }
        }

        private void BtnEditGrade_Click(object sender, RoutedEventArgs e)
        {
            if (GradesGrid.SelectedItem is null) return;
            dynamic row = GradesGrid.SelectedItem;
            int id = (int)row.Id;
            var grade = _ctx.Grades.Find(id);
            if (grade == null) return;

            var students = _ctx.Students.OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList();
            var dlg = new GradeDialog(students, grade);
            if (dlg.ShowDialog() == true)
            {
                _ctx.SaveChanges();
                LoadGrades();
            }
        }

        private void BtnDeleteGrade_Click(object sender, RoutedEventArgs e)
        {
            if (GradesGrid.SelectedItem is null) return;
            dynamic row = GradesGrid.SelectedItem;
            int id = (int)row.Id;
            var grade = _ctx.Grades.Find(id);
            if (grade == null) return;
            if (MessageBox.Show($"Удалить оценку Id={grade.Id}?", "Подтвердите", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _ctx.Grades.Remove(grade);
                _ctx.SaveChanges();
                LoadGrades();
            }
        }
        #endregion

        #region Attendance buttons
        private void BtnAddAttendance_Click(object sender, RoutedEventArgs e)
        {
            var students = _ctx.Students.OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList();
            var dlg = new AttendanceDialog(students);
            if (dlg.ShowDialog() == true)
            {
                _ctx.Attendance.Add(dlg.Attendance);
                _ctx.SaveChanges();
                LoadAttendance();
            }
        }

        private void BtnEditAttendance_Click(object sender, RoutedEventArgs e)
        {
            if (AttendanceGrid.SelectedItem is null) return;
            dynamic row = AttendanceGrid.SelectedItem;
            int id = (int)row.Id;
            var att = _ctx.Attendance.Find(id);
            if (att == null) return;
            var students = _ctx.Students.OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList();
            var dlg = new AttendanceDialog(students, att);
            if (dlg.ShowDialog() == true)
            {
                _ctx.SaveChanges();
                LoadAttendance();
            }
        }

        private void BtnDeleteAttendance_Click(object sender, RoutedEventArgs e)
        {
            if (AttendanceGrid.SelectedItem is null) return;
            dynamic row = AttendanceGrid.SelectedItem;
            int id = (int)row.Id;
            var att = _ctx.Attendance.Find(id);
            if (att == null) return;
            if (MessageBox.Show($"Удалить запись посещаемости Id={att.Id}?", "Подтвердите", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _ctx.Attendance.Remove(att);
                _ctx.SaveChanges();
                LoadAttendance();
            }
        }
        #endregion

        protected override void OnClosed(EventArgs e)
        {
            _ctx?.Dispose();
            base.OnClosed(e);
        }
    }
}
