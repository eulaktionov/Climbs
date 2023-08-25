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
    public partial class CountryDialog : BaseDialog
    {
        TextBox countryBox;
        
        public string Country
        { 
            get => countryBox.Text.Trim(); 
            set => countryBox.Text = value; 
        }

        public CountryDialog() : base()
        {
            Text = "Country";
            this.ClientSize = new Size(countryBox.Right + _shift, countryBox.Bottom + _shift + okCancelPanel.Height);
        }

        protected override void MakeControls()
        {
            countryBox = new()
            {
                Width = _length * 2,
                Top = _shift,
                Left = _shift
            };
            countryBox.TextChanged += (s, e) => ResetError(countryBox);
            controlsPanel.Controls.Add(countryBox);
        }

        protected override bool Valid()
        {
            if (string.IsNullOrWhiteSpace(countryBox.Text))
            {
                SetError(countryBox, "Country error!");
                return false;
            }
            return true;
        }
    }
}
