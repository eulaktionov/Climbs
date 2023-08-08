using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;

namespace Climbs.TableTabs
{
    internal class CountryTab : TableTab
    {
        public CountryTab(string tableName, SqlConnection connection,
            Form form) : base(tableName, connection, form)
        {
            editForm = new EditForms.CountryForm();
            editForm.Owner = form;
        }
    }
}
