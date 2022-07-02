using System;
using System.Collections.Generic;
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

namespace management_system.app.views.entity.worker
{
    /// <summary>
    /// Logika interakcji dla klasy AddWorker.xaml
    /// </summary>
    public partial class AddWorker : Window
    {
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Add(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "" || Surname.Text == "" || Salary.Text == "" || Tin.Text == "")
            {
                Notification.Content = "Error";
                return;
            }
            Database db = new Database();
            db.Open();
            Worker worker = new Worker(Name.Text, Surname.Text, Tin.Text, Convert.ToInt32(Salary.Text));
            db.em.Add(worker);
            db.em.flush();
            db.Close();

            Name.Text = Surname.Text = Salary.Text = Tin.Text = "";

            Notification.Content = "Worker has been successfully added to the database!";
        }

        public AddWorker()
        {
            InitializeComponent();
        }
    }
}
