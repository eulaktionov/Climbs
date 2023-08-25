using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using Climbs.EditForm;

using Microsoft.Data.SqlClient;

namespace Climbs.TableTab
{
    //internal abstract class Base : Panel
    internal abstract class Base : Panel
    {
        SqlDataAdapter adapter;
        DataTable table;
        DataGridView grid;

        protected EditForm.BaseDialog editForm;
        protected DataRow dataRow;

        public Base(SqlConnection connection, string tableName)
        {
            string queryText = $"Select * from {tableName}";
            adapter = new SqlDataAdapter(queryText, connection);
            new SqlCommandBuilder(adapter);
            table = new DataTable();
            adapter.Fill(table);

            grid = new()
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ShowCellToolTips = false,
                DataSource = table
            };

            Controls.Add(grid);
            Dock = DockStyle.Fill;
            grid.DataBindingComplete += (s, e) => SetGrid();
        }

        public void EnterData()
        {
            grid.Focus();
        }

        protected virtual void SetGrid()
        {
            grid.Columns["Id"].ReadOnly = true;
        }

        public void Insert()
        {
            ClearEditFormData();
            if(editForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dataRow = table.NewRow();
                    GetEditFormData();
                    table.Rows.Add(dataRow);
                    grid.Refresh();

                    grid.ClearSelection();
                    SelectLastRow();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Insert");
                }
            }
        }

        void SelectLastRow()
        {
            int rowIndex = grid.Rows.Count - 1;
            grid.Rows[rowIndex].Selected = true;
            grid.Rows[rowIndex].Cells[0].Selected = true;
            grid.CurrentCell = grid.Rows[rowIndex].Cells[0];
        }

        public void Delete()
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
                            table.Rows.Remove(selectedRow);
                        }
                        else
                        {
                            selectedRow.Delete();
                        }
                        grid.Refresh();

                        if (selectedIndex == grid.Rows.Count 
                            && selectedIndex > 0)
                        {
                            Debug.WriteLine("Last Row");
                            SelectLastRow();
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Delete");
                }
            }
        }

        public void Update()
        {
            if(grid.SelectedRows.Count > 0)
            {
                int selectedIndex = grid.SelectedRows[0].Index;
                dataRow = (grid?.Rows[selectedIndex]?.DataBoundItem as DataRowView)?.Row;
                if(dataRow != null)
                {
                    SetEditFormData();
                    if(editForm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            GetEditFormData();
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

        protected abstract void GetEditFormData();
        protected abstract void SetEditFormData();
        protected abstract void ClearEditFormData();

        public void Save()
        {
            if(adapter is not null)
            {
                MessageBox.Show("!Save!", Parent.Text);
                try
                {
                    adapter.Update(table);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Save!");
                }
            }
        }
    }
}
