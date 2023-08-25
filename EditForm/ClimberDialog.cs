using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Climbs.EditForm
{
    internal class ClimberDialog : BaseDialog
    {
        TextBox climberBox;
        TextBox addressBox;

        public string Climber
        {
            get => climberBox.Text.Trim();
            set => climberBox.Text = value;
        }

        public string Address
        {
            get => addressBox.Text.Trim();
            set => addressBox.Text = value;
        }

        public ClimberDialog() : base()
        {
            Text = "Climber";
            this.ClientSize = new Size(addressBox.Right + _shift, addressBox.Bottom + _shift + okCancelPanel.Height);
        }

        protected override void MakeControls()
        {
            climberBox = new()
            {
                Width = _length * 2,
                Top = _shift,
                Left = _shift
            };
            climberBox.TextChanged += (s, e) => ResetError(climberBox);
            controlsPanel.Controls.Add(climberBox);

            addressBox = new()
            {
                Width = _length * 2,
                Top = climberBox.Bottom + _shift,
                Left = _shift
            };
            addressBox.TextChanged += (s, e) => ResetError(addressBox);
            controlsPanel.Controls.Add(addressBox);
        }

        protected override bool Valid()
        {
            if(string.IsNullOrWhiteSpace(climberBox.Text))
            {
                SetError(climberBox, "Climber error!");
                return false;
            }

            if(string.IsNullOrWhiteSpace(addressBox.Text))
            {
                SetError(climberBox, "Address error!");
                return false;
            }

            return true;
        }
    }
}
