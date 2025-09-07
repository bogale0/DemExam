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

namespace WpfApp15
{
    /// <summary>
    /// Логика взаимодействия для AttendanceDialog.xaml
    /// </summary>
    public partial class AttendanceDialog : Window
    {
        public Attendance Attendance { get; private set; }
        private List<dynamic> _studentsForCombo;

        public AttendanceDialog(List<Students> students)
        {
            InitializeComponent();
            Attendance = new Attendance { AttendanceDate = DateTime.Today, IsPresent = true };
            PrepareStudents(students);
            DpDate.SelectedDate = Attendance.AttendanceDate;
            CbIsPresent.IsChecked = Attendance.IsPresent;
        }

        public AttendanceDialog(List<Students> students, Attendance existing)
        {
            InitializeComponent();
            Attendance = existing;
            PrepareStudents(students);
            CbStudents.SelectedValue = Attendance.StudentId;
            DpDate.SelectedDate = Attendance.AttendanceDate;
            CbIsPresent.IsChecked = Attendance.IsPresent;
            TbNote.Text = Attendance.Note;
        }

        private void PrepareStudents(List<Students> students)
        {
            _studentsForCombo = students.Select(s => (dynamic)new { Id = s.Id, FullName = s.LastName + " " + s.FirstName }).ToList();
            CbStudents.ItemsSource = _studentsForCombo;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (CbStudents.SelectedValue == null || !DpDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите студента и дату.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Attendance.StudentId = (int)CbStudents.SelectedValue;
            Attendance.AttendanceDate = DpDate.SelectedDate.Value;
            Attendance.IsPresent = CbIsPresent.IsChecked ?? false;
            Attendance.Note = string.IsNullOrWhiteSpace(TbNote.Text) ? null : TbNote.Text.Trim();

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
