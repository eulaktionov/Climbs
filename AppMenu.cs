using System.Diagnostics.PerformanceData;
using System.Windows.Forms;

using static Climbs.Properties.Resources;

namespace Climbs
{
    public partial class StartForm
    {
        MenuStrip menu;
        ToolStripMenuItem dataMenu;
        ToolStripMenuItem editMenu;

        ToolStripMenuItem oneTableData;

        ToolStripMenuItem countryData;
        ToolStripMenuItem mountainData;
        ToolStripMenuItem climbData;
        ToolStripMenuItem climberData;
        ToolStripMenuItem climbClimberData;

        ToolStripMenuItem insertCommand;
        ToolStripMenuItem deleteCommand;
        ToolStripMenuItem updateCommand;
        ToolStripMenuItem closeTabCommand;

        void CreateMenu()
        {
            menu = new MenuStrip();
            menu.Dock = DockStyle.Right;
            Controls.Add(menu);

            dataMenu = new ToolStripMenuItem("&Data");
            editMenu = new ToolStripMenuItem("&Edit");
            menu.Items.AddRange(new ToolStripMenuItem[] 
                { dataMenu, editMenu });

            oneTableData = new ToolStripMenuItem("&One Table");
            oneTableData.Click += (s, e) => ShowCountries();

            countryData = new ToolStripMenuItem("&Countries");
            countryData.Click += (s, e) => NewTab(CountriesTable);

            mountainData = new ToolStripMenuItem("&Mountains");
            mountainData.Click += (s, e) => NewTab(MountainsTable);

            climbData = new ToolStripMenuItem("&Climbs");
            climbData.Click += (s, e) => NewTab(ClimbsTable);

            climberData = new ToolStripMenuItem("&Climbers");
            climberData.Click += (s, e) => NewTab(ClimbersTable);

            climbClimberData = new ToolStripMenuItem("&Climb Climbers");
            climbClimberData.Click += (s, e) => NewTab(ClimbClimbersTable);

            dataMenu.DropDownItems.AddRange(new ToolStripItem[]
                { oneTableData, new ToolStripSeparator(),
                  climbData, climbClimberData, climberData,
                  new ToolStripSeparator(), mountainData, countryData });

            insertCommand = new ToolStripMenuItem("&Insert",
                null, (s, e) => AddCountry(), Keys.Control | Keys.I);
            deleteCommand = new ToolStripMenuItem("&Delete",
                null, (s, e) => DeleteCountry(), Keys.Delete);
            updateCommand = new ToolStripMenuItem("&Update",
                null, (s, e) => UpdateCountry(), Keys.Control | Keys.U);
            closeTabCommand = new ToolStripMenuItem("&Close Tab",
                null, (s, e) => CloseTab(), Keys.Control | Keys.C);

            editMenu.DropDownItems.AddRange(new ToolStripItem[]
                { insertCommand, deleteCommand, updateCommand,
                  new ToolStripSeparator(), closeTabCommand});
        }
    }
}