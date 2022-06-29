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

namespace management_system.app.views.entity.order
{
    /// <summary>
    /// Logika interakcji dla klasy AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Add(object sender, RoutedEventArgs e)
        {
            if (Date.SelectedDate == null || Suppliers.SelectedIndex == -1 || Items.SelectedIndex == -1 || Amount.Text == "")
            {
                Notification.Content = "Error";
                return;
            }
            Database db = new Database();
            db.Open();
            Order order = new Order(Suppliers.SelectedIndex, Items.SelectedIndex, Convert.ToInt32(Amount.Text), (DateTime)Date.SelectedDate, (bool)IsCyclic.IsChecked);
            if (!db.em.AddOrder(order))
            {
                Notification.Content = "Error";
                db.Close();
                return;
            };
            db.Close();

            Amount.Text = "";
            Suppliers.SelectedIndex = 0;
            Items.SelectedIndex = 0;
            IsCyclic.IsChecked = false;

            Notification.Content = "Order has been successfully added to the database!";
        }
        public AddOrder()
        {
            InitializeComponent();
            Database db = new Database();
            db.Open();
            List<string> suppliers = db.em.GetAllEntities("suppliers");
            List<string> items = db.em.GetAllEntities("items");
            foreach (var elem in suppliers)
                Suppliers.Items.Add(elem);
            foreach (var elem in items)
                Items.Items.Add(elem);

            db.Close();

        }
    }
}
