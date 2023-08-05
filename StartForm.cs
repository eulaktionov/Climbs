using System.Data;
using Microsoft.Data.SqlClient;
using static Climbs.Properties.Resources;
using static Climbs.Properties.Settings;

using Microsoft.Extensions.Configuration;
using System.IO;
using System.Configuration;

namespace Climbs
{
    public partial class StartForm : Form
    {
        ConfigurationRoot? configuration;
        TabControl tabControl;
        DataGridView grid;
        SqlConnection connection;

        public StartForm()
        {
            InitializeComponent();
            MakeControls();
            SetForm();
            SetConnection();
            Closed += (s, e) => Save();
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

        void SetConnection()
        {
            configuration = (ConfigurationRoot)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            string? connectionString = configuration
                .GetConnectionString("ClimbersConnection");
            connection = new SqlConnection(connectionString);

            DateTime lastSessionDate = Default.LastSessionDate;
            MessageBox.Show(lastSessionDate.ToString(), AppName);
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

        void Save()
        {
            Default.LastSessionDate = DateTime.Now;
            Default.Save();
        }
    }
}