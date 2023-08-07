using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Climbs.EditForms
{
    internal class CountryForm : EditForm
    {
        TextBox countryBox;
        public string Country { get; set; } = string.Empty;

        public CountryForm() : base()
        {
        }

        public CountryForm(string country) : this()
        {
            countryBox.Text = country;
        }

        protected override void MakeControls()
        {
            countryBox = new()
            {
                Width = _length * 2,
                Top = _shift,
                Left = _shift
            };
            countryBox.TextChanged += (s, e) 
                => ResetErrorHandler(countryBox);
            Controls.Add(countryBox);

            base.MakeControls();

            okButton.Top = countryBox.Bottom + _shift;
            okButton.Left = countryBox.Left;
            cancelButton.Top = okButton.Top;
            cancelButton.Left = okButton.Right + _shift;
        }

        protected override void SetForm()
        {
            base.SetForm();

            ClientSize = new Size(countryBox.Right + _shift,
                cancelButton.Bottom + _shift);
        }

        protected override void CheckResult()
        {
            if(string.IsNullOrWhiteSpace(countryBox.Text))
            {
                SetErrorHandler(countryBox, "Country error!");
                return;
            }

            Country = (countryBox.Text).Trim();

            base.CheckResult();
        }
    }
}
