using static Climbs.Properties.Resources;

namespace Climbs
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
            MakeControls();
            SetForm();
        }

        void MakeControls()
        {

        }

        void SetForm()
        {
            (Text, Icon) = (AppName, AppIcon);
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}