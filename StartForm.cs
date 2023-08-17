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
        SqlDataAdapter adapter;
        DataTable dataTable;

        TabControl tabControl;
        DataGridView grid;

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

        void ShowCountries()
        {
            try
            {
                string queryText = "Select * from Country";
                adapter = new SqlDataAdapter(queryText, connection);
                new SqlCommandBuilder(adapter);
                dataTable = new DataTable();
                adapter.Fill(dataTable);

                grid = new()
                {
                    Dock = DockStyle.Fill,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    ReadOnly = true,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    ShowCellToolTips = false,
                    DataSource = dataTable
                };

                TabPage tab = new TabPage("Countries");
                tabControl.Controls.Add(tab);
                tab.Controls.Add(grid);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, AppName);
            }
        }

        void Save()
        {
            if (adapter is not null)
            {
                try
                {
                    adapter.Update(dataTable);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Save!");
                }
            }

            Default.LastSessionDate = DateTime.Now;
            Default.Save();
        }

        void AddCountry()
        {
            var addForm = new CountryForm()
            {
                Owner = this
            };
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["Name"] = addForm.Country;
                    dataTable.Rows.Add(dataRow);
                    grid.Refresh();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Insert");
                }
            }
        }

        void DeleteCountry()
        {
            if(grid.SelectedRows.Count > 0)
            {
                try
                {
                    int selectedIndex = grid.SelectedRows[0].Index;
                    DataRow? selectedRow = (grid?.Rows[selectedIndex]?.DataBoundItem as DataRowView)?.Row;
                    if(selectedRow != null)
                    {
                        if(selectedRow.RowState == DataRowState.Added)
                        {
                            dataTable.Rows.Remove(selectedRow);
                        }
                        else
                        {
                            selectedRow.Delete();
                        }
                        grid.Refresh();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Delete");
                }
            }
        }

        void UpdateCountry()
        {
            if(grid.SelectedRows.Count > 0)
            {
                int selectedIndex = grid.SelectedRows[0].Index;
                DataRow selectedRow = (grid?.Rows[selectedIndex]?.DataBoundItem as DataRowView)?.Row;
                if(selectedRow != null)
                {
                    string? country = selectedRow["Name"].ToString();
                    var addForm = new CountryForm(country)
                    {
                        Owner = this,

                    };
                    if(addForm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            selectedRow["Name"] = addForm.Country;
                            grid.Refresh();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Update");
                        }
                    }
                    grid.Refresh();
                }
            }
        }

        void CloseTab()
        {
            if (tabControl.SelectedTab is not null)
            {
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
        }

        TableTab.Base CreateTab(string key)
        {
            if(key == CountriesTable)
                return new TableTab.Country(connection);
            if(key == MountainsTable)
                return new TableTab.Mountain(connection);
            if(key == ClimbsTable)
                return new TableTab.Climb(connection);
            if(key == ClimbersTable)
                return new TableTab.Climbers(connection);
            if(key == ClimbClimbersTable)
                return new TableTab.ClimbClimbers(connection);
            else
                return null;
        }
    }
}