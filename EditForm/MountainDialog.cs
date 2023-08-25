using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Climbs.EditForm
{
    public partial class MountainDialog : BaseDialog
    {
        TextBox mountainBox;
        TextBox heightBox;
        ComboBox countryList;

        public string Mountain
        {
            get => mountainBox.Text.Trim();
            set => mountainBox.Text = value;
        }

        public int MountainHeight
        {
            get => int.Parse(heightBox.Text.Trim());
            set => heightBox.Text = value.ToString();
        }

        public int CountryId
        {
            get => int.Parse(countryList.SelectedValue?.ToString());
            set => countryList.SelectedValue = value;
        }

        public MountainDialog(DataTable countries) : base()
        {
            countryList.DataSource = countries;
            countryList.DisplayMember = "Name";
            countryList.ValueMember = "Id";

            Text = "Mountain";
            this.ClientSize = new Size(mountainBox.Right + _shift, countryList.Bottom + _shift + okCancelPanel.Height);
        }

        protected override void MakeControls()
        {
            mountainBox = new()
            {
                Width = _length * 2,
                Top = _shift,
                Left = _shift
            };
            mountainBox.TextChanged += (s, e) => ResetError(mountainBox);
            controlsPanel.Controls.Add(mountainBox);

            heightBox = new()
            {
                Width = _length * 2,
                Top = mountainBox.Bottom + _shift,
                Left = _shift,
                TextAlign = HorizontalAlignment.Right
            };
            heightBox.TextChanged += (s, e) => ResetError(heightBox);
            controlsPanel.Controls.Add(heightBox);

            countryList = new()
            {
                Width = _length * 2,
                Top = heightBox.Bottom + _shift,
                Left = _shift,
            };
            controlsPanel.Controls.Add(countryList);
        }

        protected override bool Valid()
        {
            if(string.IsNullOrWhiteSpace(mountainBox.Text))
            {
                SetError(mountainBox, "Mountain error!");
                return false;
            }

            if(!int.TryParse(heightBox.Text, out _))
            {
                SetError(heightBox, "Height error!");
                return false;
            }

            if (countryList.SelectedIndex == -1)
            {
                SetError(countryList, "Country error!");
                return false;
            }

            return true;
        }
    }
}
