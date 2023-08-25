using System.Data;
using Microsoft.Data.SqlClient;
using static Climbs.Properties.Resources;
using static Climbs.Properties.Settings;

using Microsoft.Extensions.Configuration;
using System.IO;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Windows.Forms;

namespace Climbs
{
    public partial class StartForm : Form
    {
        SqlConnection connection;
        TabControl tabControl;

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
            tabControl = new()
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(tabControl);
            tabControl.Deselecting += (s, e) => GetSelectedItem()?.Save();
        }

        void SetForm()
        {
            (Text, Icon) = (AppName, AppIcon);
            StartPosition = FormStartPosition.CenterScreen;

            DateTime lastSessionDate = Default.LastSessionDate;
            Text += $" ({lastSessionDate})";
        }

        void SetConnection()
        {
            var configuration = (ConfigurationRoot)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            string? connectionString = configuration
                .GetConnectionString("ClimbersConnection");
            connection = new SqlConnection(connectionString);
        }

        void Save()
        {
            for(int i = tabControl.TabPages.Count - 1; i >= 0; i--)
            {
                GetItem(i).Save();
            }

            Default.LastSessionDate = DateTime.Now;
            Default.Save();
        }

        void Insert() => GetSelectedItem()?.Insert();
        void Delete() => GetSelectedItem()?.Delete();
        void Update() => GetSelectedItem()?.Update();

        TableTab.Base GetSelectedItem() =>
            tabControl?.SelectedTab?.Controls[0] as TableTab.Base;

        TableTab.Base GetItem(int i) =>
            tabControl?.TabPages[i]?.Controls[0] as TableTab.Base;

        void CloseTab()
        {
            if (tabControl.SelectedTab is not null)
            {
                GetSelectedItem().Save();
                tabControl.TabPages.Remove(tabControl.SelectedTab);
            }
        }

        void NewTab(string key)
        {
            if (!tabControl.TabPages.ContainsKey(key))
            {
                tabControl.TabPages.Add(key, key);
                tabControl.TabPages[key].Controls.Add(CreateTab(key));
            }
            tabControl.SelectTab(tabControl.TabPages[key]);
            GetSelectedItem()?.EnterData();
        }

        TableTab.Base CreateTab(string key)
        {
            if(key == CountriesTable)
                return new TableTab.Country(connection, this);
            if(key == MountainsTable)
                return new TableTab.Mountain(connection, this);
            if(key == ClimbersTable)
                return new TableTab.Climber(connection, this);
            if(key == ClimbsTable)
                return new TableTab.Climb(connection, this);
            if(key == ClimbClimbersTable)
                return new TableTab.ClimbClimbers(connection, this);
            else
                return null;
        }
    }
}