using management_system.app.entity;
using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace management_system
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currPage = "items";
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

        private void SearchAction(object sender, RoutedEventArgs e)
        {
            Database db = new Database();
            db.Open();

            string query = $"SELECT * FROM {currPage} ";
            SQLiteCommand cmd = new SQLiteCommand(query, db.Connection);

            SQLiteDataReader dataReader = cmd.ExecuteReader();
            query += "WHERE ";

            int max = 0;
            for (int i = 0; i < dataReader.FieldCount; i++) { max++; }

            foreach (var col in dataReader.GetValues())
            {
                query += $"{col} LIKE '%{SearchBar.Text}%' ";
                if (--max != 0)
                {
                    query += "OR ";
                }
            }

            cmd = new SQLiteCommand(query, db.Connection);
            dataReader = cmd.ExecuteReader();
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

        public void RenderReport(object sender, RoutedEventArgs e)
        {
            Database db = new Database();
            db.Open();

            // dd/mm/yyyy
            DateTime lastMonth = DateTime.Today.AddMonths(-1);

            string query = $"SELECT * FROM orders";

            SQLiteCommand cmd = new SQLiteCommand(query, db.Connection);

            SQLiteDataReader reader = cmd.ExecuteReader();
            StringBuilder sb = new StringBuilder();

            var columnNames = Enumerable.Range(0, reader.FieldCount)
                                    .Select(reader.GetName) 
                                    .ToList();

            sb.Append(string.Join(",", columnNames));

            sb.AppendLine();

            StreamWriter sw = new StreamWriter("../../../report.csv");

            while (reader.Read())
            {
                if (Convert.ToDateTime(reader[4]) > lastMonth) {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string value = reader[i].ToString();
                        if (value.Contains(","))
                            value = "\"" + value + "\"";

                        sb.Append(value.Replace(Environment.NewLine, " ") + ",");
                    }
                    sb.Length--;
                    sb.AppendLine();
                }
            }

            sw.Write(sb.ToString());
            sw.Close();
            db.Close();

            var tooltip = new ToolTip { Content = "Report generated" };
            FlashMsg.ToolTip = tooltip;

            tooltip.IsOpen = true;
            tooltip.StaysOpen = false;
        }

        public void RecoverDB(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            File.Delete("../../../data/database.sqlite");
            File.Copy("../../../data/backup.sqlite", "../../../data/database.sqlite");

            var tooltip = new ToolTip { Content = "Report generated" };
            FlashMsg.ToolTip = tooltip;

            tooltip.IsOpen = true;
            tooltip.StaysOpen = false;
        }
    }
}