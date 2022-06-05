using management_system.app.entity;
using System;
using System.Data.SQLite;
using System.Windows;

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
        }

        private void ShowWorkers(object sender, RoutedEventArgs e)
        {
            Database db = new Database();
            db.Open();
            SQLiteDataReader dataReader = db.GetAllData("workers");

            DataListView.Items.Clear();
            while (dataReader.Read())
            {
                DataListView.Items.Add($"{dataReader[0]} {dataReader[1]} {dataReader[2]}");
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
                DataListView.Items.Add($"{dataReader[0]} {dataReader[1]} {dataReader[2]}");
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
                DataListView.Items.Add(String.Format($"{dataReader[0]} {dataReader[1]} {dataReader[2]}"));
            }
            
            db.Close();
        }
    }
}