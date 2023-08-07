using System.Data;
using Microsoft.Data.SqlClient;
using static Climbs.Properties.Resources;
using static Climbs.Properties.Settings;

using Microsoft.Extensions.Configuration;
using System.IO;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;

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
                grid.Columns["Id"].Visible = false;
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
            var addForm = new EditForms.CountryForm()
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
                    DataRow selectedRow = GetDataRow(selectedIndex);
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
                DataRow selectedRow = GetDataRow(selectedIndex);
                if(selectedRow != null)
                {
                    string? country = selectedRow["Name"].ToString();
                    var addForm = new EditForms.CountryForm(country)
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

        DataRow GetDataRow(int gridIndex)
        {
            DataRowView selectedRowView = grid.Rows[gridIndex]
                .DataBoundItem as DataRowView;
            if(selectedRowView != null)
            {
                return selectedRowView.Row;
            }
            else return null;
        }
    }
}