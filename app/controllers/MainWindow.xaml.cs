using management_system.app.entity;
using management_system.app.views.entity.item;
using management_system.app.views.entity.order;
using management_system.app.views.entity.worker;
using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace management_system
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currPage;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;

            Database db = new Database();
            db.Open();
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

        private void ShowTable(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            currPage = button.Tag.ToString();

            Database db = new Database();
            db.Open();
            SQLiteDataReader dataReader = db.GetAllData(currPage);

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

            int max = dataReader.FieldCount;

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



        public void RenderReport(object sender, RoutedEventArgs e)
        {
            Database db = new Database();
            db.Open();
            var button = sender as Button;
            var page = button.Tag.ToString();

            // dd/mm/yyyy
            DateTime lastMonth = DateTime.Today.AddMonths(-1);

            string query = $"SELECT * FROM {page}";

            SQLiteCommand cmd = new SQLiteCommand(query, db.Connection);

            SQLiteDataReader reader = cmd.ExecuteReader();
            StringBuilder sb = new StringBuilder();

            var columnNames = Enumerable.Range(0, reader.FieldCount)
                                    .Select(reader.GetName) 
                                    .ToList();

            sb.Append(string.Join(",", columnNames));

            sb.AppendLine();

            StreamWriter sw = new StreamWriter($"../../../report_{page}_{DateTime.Now.ToString("MM/dd/yyyy_HHmmss")}.csv");

            while (reader.Read())
            {
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

            var tooltip = new ToolTip { Content = "Database recovered" };
            FlashMsg.ToolTip = tooltip;

            tooltip.IsOpen = true;
            tooltip.StaysOpen = false;
        }

        public void AddEntity(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            currPage = button.Tag.ToString();

            Window page = new AddItem();
            switch (currPage)
            {
                case "items":
                    page = new AddItem();
                    break;

                case "orders":
                    page = new AddOrder();
                    break;

                case "workers":
                    page = new AddWorker();
                    break;

                default:
                    break;
            }

            page.Show();
        }
    }
}