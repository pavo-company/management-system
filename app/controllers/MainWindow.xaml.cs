using management_system.app.entity;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace management_system
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ListView dataListView;
        public MainWindow()
        {
            InitializeComponent();
            dataListView = DataListView;
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

        public void ShowResults(object sender, RoutedEventArgs e, SQLiteDataReader dataReader)
        {
            
        }

        private void SearchAction(object sender, RoutedEventArgs e)
        {
            Database db = new Database();

            string prahse = $"SELECT * FROM items WHERE name LIKE '%{SearchBar.Text}%'";
            SQLiteCommand cmd = new SQLiteCommand(prahse, db.Connection);

            db.Open();
            SQLiteDataReader dataReader = cmd.ExecuteReader();
            DataListView.Items.Clear();
            while (dataReader.Read())
            {
                DataListView.Items.Add($"{dataReader[0]} {dataReader[1]} {dataReader[2]}");
            }
            db.Close();

        }
    }
}