using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Climbs.EditForm
{
    internal class ClimberClimbDialog : BaseDialog
    {
        ComboBox climbList;
        ListBox climberList;

        public int ClimbId
        {
            get => int.Parse(climbList.SelectedValue?.ToString());
            set => climbList.SelectedValue = value;
        }

        public int ClimberId
        {
            get => int.Parse(climberList.SelectedValue?.ToString());
            set => climberList.SelectedValue = value;
        }

        public ClimberClimbDialog(DataTable climbs, DataTable climbers) : base()
        {
            climbList.DataSource = climbs;
            climbList.DisplayMember = "Id";
            climbList.ValueMember = "Id";

            climberList.DataSource = climbers;
            climberList.DisplayMember = "Name";
            climberList.ValueMember = "Id";

            Text = "Climb Climbers";
            this.ClientSize = new Size(climberList.Right + _shift, climberList.Bottom + _shift + okCancelPanel.Height);
        }

        protected override void MakeControls()
        {
            climbList = new()
            {
                Width = _length * 2,
                Height = _length,
                Top = _shift,
                Left = _shift,
            };
            controlsPanel.Controls.Add(climbList);

            climberList = new()
            {
                Width = _length * 2,
                Height = _length,
                Top = climbList.Bottom + _shift,
                Left = _shift,
            };
            controlsPanel.Controls.Add(climberList);
        }

        protected override bool Valid()
        {
            if(climbList.SelectedIndex == -1)
            {
                SetError(climbList, "Climb error!");
                return false;
            }

            if(climberList.SelectedIndex == -1)
            {
                SetError(climbList, "Climber error!");
                return false;
            }

            return true;
        }
    }
}
