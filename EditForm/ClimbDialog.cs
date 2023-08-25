using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace Climbs.EditForm
{
    internal class ClimbDialog : BaseDialog
    {
        DateTimePicker startDate;
        DateTimePicker endDate;
        ListBox montainList;

        public DateTime Start
        {
            get => startDate.Value.Date;
            set => startDate.Value = value;
        }

        public DateTime End
        {
            get => endDate.Value.Date;
            set => endDate.Value = value;
        }

        public int MountainId
        {
            get => int.Parse(montainList.SelectedValue?.ToString());
            set => montainList.SelectedValue = value;
        }

        public ClimbDialog(DataTable mountains) : base()
        {
            montainList.DataSource = mountains;
            montainList.DisplayMember = "Name";
            montainList.ValueMember = "Id";

            Text = "Climb";
            this.ClientSize = new Size(montainList.Right + _shift, montainList.Bottom + _shift + okCancelPanel.Height);
        }

        protected override void MakeControls()
        {
            startDate = new()
            {
                Width = _length * 2,
                Top = _shift,
                Left = _shift
            };
            startDate.ValueChanged += (s, e) => ResetError(startDate);
            controlsPanel.Controls.Add(startDate);

            endDate = new()
            {
                Width = _length * 2,
                Top = startDate.Bottom + _shift,
                Left = _shift
            };
            startDate.ValueChanged += (s, e) => ResetError(endDate);
            controlsPanel.Controls.Add(endDate);

            montainList = new()
            {
                Width = _length * 2,
                Height = _length,
                Top = endDate.Bottom + _shift,
                Left = _shift,
            };
            controlsPanel.Controls.Add(montainList);
        }

        protected override bool Valid()
        {
            if(montainList.SelectedIndex == -1)
            {
                SetError(montainList, "Mountain error!");
                return false;
            }

            return true;
        }
    }
}
