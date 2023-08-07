using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Climbs.EditForms
{
    public partial class EditForm : Form
    {
        protected int _length = 200;
        protected int _shift = 20;

        protected Button okButton;
        protected Button cancelButton;
        protected ErrorProvider errorHandler;

        public EditForm()
        {
            InitializeComponent();
            MakeControls();
            SetForm();
        }

        protected virtual void MakeControls()
        {
            MakeButtons();
            errorHandler = new(this);
        }

        void MakeButtons()
        {
            okButton = new()
            {
                Text = "Ok",
                Width = _shift * 4,
                Height = (int)(_shift * 1.5)
            };
            okButton.Click += (s, e) => CheckResult();
            Controls.Add(okButton);

            cancelButton = new()
            {
                Text = "Cancel",
                Width = _shift * 4,
                Height = (int)(_shift * 1.5),
                DialogResult = DialogResult.Cancel
            };
            Controls.Add(cancelButton);
        }

        protected virtual void SetForm()
        {
            AcceptButton = okButton;
            CancelButton = cancelButton;

            StartPosition = FormStartPosition.CenterParent;
        }

        protected virtual void CheckResult()
        {
            DialogResult = DialogResult.OK;
        }

        protected void SetErrorHandler(Control control, string message)
        {
            errorHandler.SetIconAlignment(control, ErrorIconAlignment.MiddleLeft);
            errorHandler.SetError(control, message);
            control.Focus();
        }

        protected void ResetErrorHandler(Control control)
        {
            errorHandler.SetError(control, "");
            errorHandler.Clear();
        }
    }
}
