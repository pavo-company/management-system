using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace management_system
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            Background = new SolidColorBrush(Color.FromRgb(150,150,150));
            DataListView.Background = new SolidColorBrush(Color.FromRgb(150, 150, 150));
        }

        private void ShowWorkers(object sender, RoutedEventArgs e)
        {
            Database db = new Database();
            db.Open();
            SQLiteDataReader dataReader = db.GetAllData("workers");

            DataListView.Items.Clear();
            while (dataReader.Read())
            {
                DataListView.Items.Add(String.Format("{0} {1} {2}", dataReader[0], dataReader[1], dataReader[2]));
            }
            
            db.Close();
        }

        private void ShowItems(object sender, RoutedEventArgs e)
        {
            Database db = new Database();
            db.Open();
            SQLiteDataReader dataReader = db.GetAllData("items");

            DataListView.Items.Clear();
            while (dataReader.Read())
            {
                DataListView.Items.Add(String.Format("{0} {1} {2}", dataReader[0], dataReader[1], dataReader[2]));
            }
            
            db.Close();
        }

        private void ShowOrders(object sender, RoutedEventArgs e)
        {
            Database db = new Database();
            db.Open();
            SQLiteDataReader dataReader = db.GetAllData("orders");

            DataListView.Items.Clear();
            while (dataReader.Read())
            {
                DataListView.Items.Add(String.Format($"{dataReader[0]} {dataReader[1]} {dataReader[2]}" ));
            }
            
            db.Close();
        }
    }
}