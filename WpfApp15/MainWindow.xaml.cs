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
        private StudentDBEntities _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new StudentDBEntities();
            LoadData();
        }

        private void LoadData()
        {
            StudentsGrid.ItemsSource = _context.Students.ToList();
            GradesGrid.ItemsSource = _context.Grades.ToList();
            AttendanceGrid.ItemsSource = _context.Attendance.ToList();
        }

        private void AddStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            var student = new Students { FirstName = "Имя", LastName = "Фамилия", BirthDate = System.DateTime.Now };
            _context.Students.Add(student);
            _context.SaveChanges();
            LoadData();
        }

        private void DeleteStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsGrid.SelectedItem is Students student)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
                LoadData();
            }
        }

        private void AddGradeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_context.Students.Any() && _context.Subjects.Any())
            {
                var grade = new Grades
                {
                    StudentId = _context.Students.First().Id,
                    SubjectId = _context.Subjects.First().Id,
                    Grade = 5,
                    GradeDate = System.DateTime.Now
                };
                _context.Grades.Add(grade);
                _context.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("Нет студентов или предметов для выставления оценки.");
            }
        }

        private void AddAttendanceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_context.Students.Any() && _context.Subjects.Any())
            {
                var att = new Attendance
                {
                    StudentId = _context.Students.First().Id,
                    SubjectId = _context.Subjects.First().Id,
                    Date = System.DateTime.Now,
                    IsPresent = true
                };
                _context.Attendance.Add(att);
                _context.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("Нет студентов или предметов для отметки посещаемости.");
            }
        }
    }
}
