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
    /// Логика взаимодействия для StudentDialog.xaml
    /// </summary>
    public partial class StudentDialog : Window
    {
        public Students Student { get; private set; }

        public StudentDialog()
        {
            InitializeComponent();
            Student = new Students();
            DataContext = Student;
        }

        public StudentDialog(Students existing) : this()
        {
            Student = existing;
            DataContext = Student;
            TbFirstName.Text = Student.FirstName;
            TbLastName.Text = Student.LastName;
            DpDob.SelectedDate = Student.DateOfBirth;
            TbEmail.Text = Student.Email;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbLastName.Text) || string.IsNullOrWhiteSpace(TbFirstName.Text))
            {
                MessageBox.Show("Имя и фамилия обязательны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Student.FirstName = TbFirstName.Text.Trim();
            Student.LastName = TbLastName.Text.Trim();
            Student.DateOfBirth = DpDob.SelectedDate;
            Student.Email = string.IsNullOrWhiteSpace(TbEmail.Text) ? null : TbEmail.Text.Trim();

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
