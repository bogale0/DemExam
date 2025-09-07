using System;
using System.Collections.Generic;
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

namespace WpfApp15
{
    /// <summary>
    /// Логика взаимодействия для GradeDialog.xaml
    /// </summary>
    public partial class GradeDialog : Window
    {
        public Grades Grade { get; private set; }
        private List<dynamic> _studentsForCombo;

        // Для добавления
        public GradeDialog(List<Students> students)
        {
            InitializeComponent();
            Grade = new Grades { GradeDate = DateTime.Today };
            PrepareStudents(students);
        }

        // Для редактирования
        public GradeDialog(List<Students> students, Grades existing)
        {
            InitializeComponent();
            Grade = existing;
            PrepareStudents(students);
            CbStudents.SelectedValue = Grade.StudentId;
            TbSubject.Text = Grade.Subject;
            TbScore.Text = Grade.Score.ToString();
        }

        private void PrepareStudents(List<Students> students)
        {
            // Создаем временные объекты с FullName для ComboBox
            _studentsForCombo = students.Select(s => (dynamic)new { Id = s.Id, FullName = s.LastName + " " + s.FirstName }).ToList();
            CbStudents.ItemsSource = _studentsForCombo;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (CbStudents.SelectedValue == null || string.IsNullOrWhiteSpace(TbSubject.Text) || !int.TryParse(TbScore.Text, out int score))
            {
                MessageBox.Show("Заполните все поля корректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Grade.StudentId = (int)CbStudents.SelectedValue;
            Grade.Subject = TbSubject.Text.Trim();
            Grade.Score = score;
            Grade.GradeDate = DateTime.Today;

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
