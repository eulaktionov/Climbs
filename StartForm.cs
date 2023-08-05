using System.Data;
using Microsoft.Data.SqlClient;
using static Climbs.Properties.Resources;

namespace Climbs
{
    public partial class StartForm : Form
    {
        TabControl tabControl;
        DataGridView grid;

        string connectionString;
        SqlConnection connection;

        public StartForm()
        {
            InitializeComponent();
            MakeControls();
            SetForm();

            connectionString =
                @"Data Source=(localdb)\MSSQLLocalDB;"
                + "Initial Catalog=Climbers";
            connection = new SqlConnection(connectionString);
        }

        void MakeControls()
        {
            CreateMenu();

            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            Controls.Add(tabControl);
        }

        void SetForm()
        {
            (Text, Icon) = (AppName, AppIcon);
            StartPosition = FormStartPosition.CenterScreen;
        }

        void ShowCountries()
        {
            string queryText = "Select * from Country";
            SqlDataAdapter adapter = new SqlDataAdapter(queryText, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            grid = new DataGridView();
            grid.Dock = DockStyle.Fill;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.DataSource = dataTable;

            TabPage tab = new TabPage("Countries");
            tabControl.Controls.Add(tab);
            tab.Controls.Add(grid);
        }
    }
}