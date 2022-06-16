using System.Windows.Controls;
using System.Data.SQLite;

namespace management_system.app.views.Controls.SearchBox
{
    public partial class SearchBox : UserControl
    {
        public SearchBox()
        {
            InitializeComponent();
        }

        private void SearchAction(object sender, System.Windows.RoutedEventArgs e)
        {
            string prahse = $"SELECT * FROM items WHERE name LIKE {SearchBar.Text}";
            SQLiteCommand cmd = new SQLiteCommand(prahse);
            cmd.ExecuteNonQuery();

        }
    }
}