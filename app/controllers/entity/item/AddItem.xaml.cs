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

namespace management_system.app.views.entity.item
{
    /// <summary>
    /// Logika interakcji dla klasy AddItem.xaml
    /// </summary>
    public partial class AddItem : Window
    {
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "" || Amount.Text == "" || Minimum.Text == "" || Price.Text == "")
            {
                Notification.Content = "Error";
                return;
            }
            Database db = new Database();
            db.Open();
            Item item = new Item(Name.Text, Convert.ToInt32(Amount.Text), Convert.ToInt32(Minimum.Text), Convert.ToInt32(Price.Text));
            db.em.Add(item);
            db.em.flush();
            db.Close();

            Name.Text = Amount.Text = Minimum.Text = Price.Text = "";

            Notification.Content = "Item has been successfully added to the database!";
        }

        public AddItem()
        {
            InitializeComponent();
        }
    }
}
