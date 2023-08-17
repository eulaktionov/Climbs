using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Data.SqlClient;

namespace Climbs.TableTab
{
    internal class Base : Panel
    {
        SqlDataAdapter adapter;
        DataTable table;
        DataGridView grid;

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

        protected virtual void SetGrid()
        {
            grid.Columns["Id"].Visible = false;
        }
    }
}
