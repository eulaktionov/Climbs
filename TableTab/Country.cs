using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Climbs.EditForm;

using Microsoft.Data.SqlClient;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static Climbs.Properties.Resources;


namespace Climbs.TableTab
{
    internal class Country : Base
    {
        public Country(SqlConnection connection, Form parent) : 
            base(connection, CountriesTable)
        {
            editForm = new EditForm.CountryDialog();
            editForm.Owner = parent;
        }

        protected override void GetEditFormData()
        {
            dataRow["Name"] = (editForm as EditForm.CountryDialog).Country;
        }

        protected override void SetEditFormData()
        {
            (editForm as EditForm.CountryDialog).Country = dataRow["Name"].ToString();
        }
        
        protected override void ClearEditFormData()
        {
            (editForm as EditForm.CountryDialog).Country = string.Empty;
        }
    }

    internal class Mountain : Base
    {
        public Mountain(SqlConnection connection, Form parent) : base(connection, MountainsTable)
        {
            string queryText = "Select * from Country";
            SqlDataAdapter adapter = new (queryText, connection);
            DataTable table = new ();
            adapter.Fill(table);

            editForm = new EditForm.MountainDialog(table);
            editForm.Owner = parent;
        }

        protected override void GetEditFormData()
        {
            dataRow["Name"] = (editForm as EditForm.MountainDialog).Mountain;
            dataRow["Height"] = (editForm as EditForm.MountainDialog).MountainHeight;
            dataRow["CountryId"] = (editForm as EditForm.MountainDialog).CountryId;
        }

        protected override void SetEditFormData()
        {
            (editForm as EditForm.MountainDialog).Mountain = dataRow["Name"].ToString();
            (editForm as EditForm.MountainDialog).MountainHeight = int.Parse(dataRow["Height"].ToString());
            (editForm as EditForm.MountainDialog).CountryId = int.Parse(dataRow["CountryId"].ToString());
        }

       protected override void ClearEditFormData()
       {
            (editForm as EditForm.MountainDialog).Mountain = string.Empty;
            (editForm as EditForm.MountainDialog).MountainHeight = 0;
        }
    }

    internal class Climber : Base
    {
        public Climber(SqlConnection connection, Form parent) : base(connection, ClimbersTable)
        {
            editForm = new EditForm.ClimberDialog();
            editForm.Owner = parent;
        }

        protected override void GetEditFormData()
        {
            dataRow["Name"] = (editForm as EditForm.ClimberDialog).Climber;
            dataRow["Address"] = (editForm as EditForm.ClimberDialog).Address;
        }

        protected override void SetEditFormData()
        {
            (editForm as EditForm.ClimberDialog).Climber = dataRow["Name"].ToString();
            (editForm as EditForm.ClimberDialog).Address = dataRow["Address"].ToString(); 
        }

        protected override void ClearEditFormData()
        {
            (editForm as EditForm.ClimberDialog).Climber = string.Empty;
            (editForm as EditForm.ClimberDialog).Address = string.Empty;
        }
    }

    internal class Climb : Base
    {
        public Climb(SqlConnection connection, Form parent) : base(connection, ClimbsTable)
        {
            string queryText = "Select * from Mountain";
            SqlDataAdapter adapter = new(queryText, connection);
            DataTable table = new();
            adapter.Fill(table);

            editForm = new EditForm.ClimbDialog(table);
            editForm.Owner = parent;
        }

        protected override void GetEditFormData()
        {
            dataRow["Start"] = (editForm as EditForm.ClimbDialog).Start;
            dataRow["End"] = (editForm as EditForm.ClimbDialog).End;
            dataRow["MountainId"] = (editForm as EditForm.ClimbDialog).MountainId;
        }

        protected override void SetEditFormData()
        {
            (editForm as EditForm.ClimbDialog).Start = (DateTime)dataRow["Start"];
            (editForm as EditForm.ClimbDialog).End = (DateTime)dataRow["End"];
            (editForm as EditForm.ClimbDialog).MountainId = int.Parse(dataRow["MountainId"].ToString());
        }

        protected override void ClearEditFormData()
        {
            (editForm as EditForm.ClimbDialog).Start = DateTime.Now;
            (editForm as EditForm.ClimbDialog).End = DateTime.Now;
        }
    }

    internal class ClimbClimbers : Base
    {
        public ClimbClimbers(SqlConnection connection, Form parent) : base(connection, ClimbClimbersTable)
        {
            string queryText = "Select * from Climb";
            SqlDataAdapter adapter = new(queryText, connection);
            DataTable climbs = new();
            adapter.Fill(climbs);

            queryText = "Select * from Climber";
            adapter = new(queryText, connection);
            DataTable climbers = new();
            adapter.Fill(climbers);

            editForm = new EditForm.ClimberClimbDialog(climbs, climbers);
            editForm.Owner = parent;
        }

        protected override void GetEditFormData()
        {
            dataRow["ClimbId"] = (editForm as EditForm.ClimberClimbDialog).ClimbId;
            dataRow["ClimberId"] = (editForm as EditForm.ClimberClimbDialog).ClimberId;
        }

        protected override void SetEditFormData()
        {
            (editForm as EditForm.ClimberClimbDialog).ClimbId = int.Parse(dataRow["ClimbId"].ToString());
            (editForm as EditForm.ClimberClimbDialog).ClimberId = int.Parse(dataRow["ClimberId"].ToString());
        }

        protected override void ClearEditFormData()
        {
        }
    }
}
