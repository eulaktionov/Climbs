using System.Diagnostics.PerformanceData;
using System.Windows.Forms;

namespace Climbs
{
    public partial class StartForm
    {
        MenuStrip menu;
        ToolStripMenuItem dataMenu;
        ToolStripMenuItem editMenu;

        ToolStripMenuItem countryData;
        ToolStripMenuItem mountainData;

        ToolStripMenuItem insertCommand;
        ToolStripMenuItem deleteCommand;
        ToolStripMenuItem updateCommand;

        void CreateMenu()
        {
            menu = new MenuStrip();
            menu.Dock = DockStyle.Right;
            Controls.Add(menu);

            dataMenu = new ToolStripMenuItem("&Data");
            editMenu = new ToolStripMenuItem("&Edit");
            menu.Items.AddRange(new ToolStripMenuItem[] 
                { dataMenu, editMenu });

            countryData = new ToolStripMenuItem("&Countries");
            countryData.Click += (s, e) => ShowCountries();
            mountainData = new ToolStripMenuItem("&Mountains");
            dataMenu.DropDownItems.AddRange(new ToolStripMenuItem[]
                { countryData, mountainData });

            insertCommand = new ToolStripMenuItem("&Insert");
            deleteCommand = new ToolStripMenuItem("&Delete");
            updateCommand = new ToolStripMenuItem("&Update");
            editMenu.DropDownItems.AddRange(new ToolStripMenuItem[]
                { insertCommand, deleteCommand, updateCommand });
        }
    }
}