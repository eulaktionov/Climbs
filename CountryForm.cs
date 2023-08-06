using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Climbs
{
    public partial class CountryForm : Form
    {
        int _length = 200;
        int _shift = 20;
        
        TextBox countryBox;
        Button okButton;
        Button cancelButton;
        ErrorProvider errorHandler;

        public string Country { get; set; } 

        public CountryForm()
        {
            InitializeComponent();
            MakeControls();
            SetForm();
        }

        public CountryForm(string country) : this()
        {
            countryBox.Text = country;
        }

        void MakeControls()
        {
            countryBox = new()
            {
                Width = _length * 2,
                Top = _shift,
                Left = _shift
            };
            countryBox.TextChanged += (s, e) => ResetErrors();
            Controls.Add(countryBox);

            errorHandler = new(this);
            MakeButtons();
        }

        void MakeButtons()
        {
            okButton = new()
            {
                Text = "Ok",
                Width = _shift * 4,
                Height = (int)(_shift * 1.5),
                Top = countryBox.Bottom + _shift,
                Left = countryBox.Left
            };
            okButton.Click += (s, e) => CheckResult();
            Controls.Add(okButton);

            cancelButton = new()
            {
                Text = "Cancel",
                Width = _shift * 4,
                Height = (int)(_shift * 1.5),
                Top = okButton.Top,
                Left = okButton.Right + 2 *_shift,
                DialogResult = DialogResult.Cancel
            };
            Controls.Add(cancelButton);
        }

        void SetForm()
        {
            AcceptButton = okButton;
            CancelButton = cancelButton;

            int width = countryBox.Right + _shift;
            int height = cancelButton.Bottom + _shift;
            ClientSize = new Size(width, height);
            
            StartPosition = FormStartPosition.CenterParent;
        }

        void CheckResult()
        {
            if (string.IsNullOrWhiteSpace(countryBox.Text))
            {
                errorHandler.SetIconAlignment(countryBox, ErrorIconAlignment.MiddleLeft);
                errorHandler.SetError(countryBox, "Country error!");
                countryBox.Focus();
                return;
            }

            Country = (countryBox.Text).Trim();
            DialogResult = DialogResult.OK;
        }

        void ResetErrors()
        {
            errorHandler.SetError(countryBox, "");
            errorHandler.Clear();
        }
    }
}
