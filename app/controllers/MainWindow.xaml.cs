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
        private string currPage = "";
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void ShowWorkers(object sender, RoutedEventArgs e)
        {
            currPage = "workers";
            Database db = new Database();
            db.Open();
            SQLiteDataReader dataReader = db.GetAllData("workers");

            ShowResults(sender, e, dataReader);
            db.Close();
        }

        private void ShowItems(object sender, RoutedEventArgs e)
        {
            currPage = "items";
            Database db = new Database();
            db.Open();
            SQLiteDataReader dataReader = db.GetAllData("items");

            ShowResults(sender, e, dataReader);

            db.Close();
        }

        private void ShowOrders(object sender, RoutedEventArgs e)
        {
            currPage = "orders";
            Database db = new Database();
            db.Open();
            SQLiteDataReader dataReader = db.GetAllData("orders");

            ShowResults(sender, e, dataReader);
            
            db.Close();
        }

        public void ShowResults(object sender, RoutedEventArgs e, SQLiteDataReader dataReader)
        {
            DataListView.Items.Clear();
            while (dataReader.Read())
            {
                string query = "";
                foreach (var el in dataReader.GetValues()) 
                {
                    query += dataReader[el.ToString()] + " ";
                }
                DataListView.Items.Add(query);
            }
        }

        private void SearchAction(object sender, RoutedEventArgs e)
        {
            Database db = new Database();

            string prahse = $"SELECT * FROM {currPage} WHERE name LIKE '%{SearchBar.Text}%'";
            SQLiteCommand cmd = new SQLiteCommand(prahse, db.Connection);

            db.Open();
            SQLiteDataReader dataReader = cmd.ExecuteReader();
            ShowResults(sender, e, dataReader);
            db.Close();

        }
    }
}