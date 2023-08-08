using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Data.SqlClient;

namespace Climbs.TableTabs
{
    internal class TableTab : TabPage
    {
        SqlDataAdapter adapter;
        DataTable dataTable;
        DataGridView grid;
        protected EditForms.EditForm editForm;

        public TableTab(string tableName, SqlConnection connection,
            Form form) : base(tableName)
        {
            string queryText = $"Select * from {tableName}";
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

            Controls.Add(grid);
            grid.VisibleChanged += Grid_VisibleChanged;
            
        }

        private void Grid_VisibleChanged(object? sender, EventArgs e)
        {
            if (grid.Visible)
            {
                grid.Columns["Id"].Visible = false;
            }
        }

        public void Save()
        {
            if(adapter is not null)
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
        }
        void Insert()
        {
            //var addForm = new EditForms.CountryForm()
            //{
            //    Owner = this
            //};
            if(editForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataRow dataRow = dataTable.NewRow();
                    FillRow(dataRow);
                    dataTable.Rows.Add(dataRow);
                    grid.Refresh();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Insert");
                }
            }
        }

        protected virtual void FillRow(DataRow dataRow)
        {
            //dataRow["Name"] = editForm.Country;
        }

        protected virtual void FillForm(DataRow dataRow)
        {
            //editForm.Country = dataRow["Name"];
        }

        void Delete()
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

        void Update()
        {
            if(grid.SelectedRows.Count > 0)
            {
                int selectedIndex = grid.SelectedRows[0].Index;
                DataRow selectedRow = GetDataRow(selectedIndex);
                if(selectedRow != null)
                {
                    //string? country = selectedRow["Name"].ToString();
                    //var addForm = new EditForms.CountryForm(country)
                    //{
                    //    Owner = this,
                    //};
                    FillForm(selectedRow);
                    if(editForm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            //selectedRow["Name"] = addForm.Country;
                            FillRow(selectedRow);
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
