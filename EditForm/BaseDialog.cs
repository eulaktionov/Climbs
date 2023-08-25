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
    public abstract partial class BaseDialog : Form
    {
        protected int _length = 200;
        protected int _shift = 20;

        protected Panel okCancelPanel;
        protected Panel controlsPanel;
        protected ErrorProvider errorHandler;

        public BaseDialog()
        {
            InitializeComponent();

            controlsPanel = new()
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(controlsPanel);
            MakeControls();
            okCancelPanel = MakeButtons();
            Controls.Add(okCancelPanel);

            errorHandler = new(this);

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
        }

        protected abstract void MakeControls();

        Panel MakeButtons()
        {
            Button okButton = new()
            {
                Text = "Ok",
                Width = _shift * 4,
                Height = (int)(_shift * 1.5),
                Dock = DockStyle.Left,
                Top = _shift
            };
            okButton.Click += (s, e) =>
                DialogResult = Valid() ? DialogResult.OK : DialogResult.None;

            Button cancelButton = new()
            {
                Text = "Cancel",
                Width = _shift * 4,
                Height = (int)(_shift * 1.5),
                Dock = DockStyle.Right,
                Top = okButton.Top,
                DialogResult = DialogResult.Cancel
            };

            Panel panel = new()
            {
                Dock = DockStyle.Bottom,
                Height = okButton.Height
            };

            AcceptButton = okButton;
            CancelButton = cancelButton;

            panel.Controls.Add(okButton);
            panel.Controls.Add(cancelButton);
            return panel;
        }

        protected abstract bool Valid();

        protected void SetError(Control control, string message)
        {
            errorHandler.SetIconAlignment(control, ErrorIconAlignment.MiddleLeft);
            errorHandler.SetError(control, message);
            control.Focus();
        }

        protected void ResetError(Control control)
        {
            errorHandler.SetError(control, "");
            errorHandler.Clear();
        }
    }
}
