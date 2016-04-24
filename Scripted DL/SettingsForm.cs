using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScriptedDL
{
    public partial class SettingsForm : Form
    {
        public SettingsForm ( )
        {
            InitializeComponent ( );
            txtSavePath.Text = Settings.Save_Path;
        }

        private void btnSelectSavePath_Click_1 ( Object sender, EventArgs e )
        {
            var fsd = new FolderSelectDialog { Title = "Universal Downloader | Save Folder" };

        request:
            var res = fsd.ShowDialog ( );

            if ( !res )
                goto request;

            txtSavePath.Text = fsd.FileName;
            Settings.Save_Path = fsd.FileName;
            Settings.Save ( );
        }
    }
}
