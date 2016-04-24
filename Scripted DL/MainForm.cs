using ScriptedDL.ScriptLoader;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace ScriptedDL
{
    public partial class MainForm : Form
    {
        private ImageList il;
        private InfoParser.ScriptInfo si;
        Boolean IsDownloading;

        public MainForm ( )
        {
            InitializeComponent ( );
            FormClosed += ( s, e ) => Application.Exit ( );

            il = new ImageList ( );
            scriptTree.ImageList = il;

            lblName.Text = lblAuthor.Text = lblFormat.Text = lblHomepage.Text = "";
        }

        private void MainForm_Load ( Object sender, EventArgs e )
        {
            // Finds all scripts
            var scripts = ScriptLoader.ScriptLoader.GetScripts ( "Scripts" );

            if(scripts == null)
            {
                ErrorMessage ( "Looks like you don't have a 'Scripts' folder!" );
                Application.Exit ( );
            }

            // And adds them 1 by 1 to the TreeList
            for ( var i = 0 ; i < scripts.Length ; i++ )
            {
                // Gets the icon
                var icon = ShellIcon.GetSmallIcon ( scripts[i] );
                // Adds to the image list
                il.Images.Add ( icon );
                // And then adds the node to the Tree the icon
                scriptTree.Nodes.Add ( new TreeNode ( Path.GetFileName ( scripts[i] ), i, i ) );
            }
        }

        private void lblHomepage_LinkClicked ( Object sender, LinkLabelLinkClickedEventArgs e )
        {
            // When the link label is clicked, open the URL on the default browser
            System.Diagnostics.Process.Start ( ( ( LinkLabel ) sender ).Text );
        }

        // When a script is selected
        private void scriptTree_AfterSelect ( Object sender, TreeViewEventArgs e )
        {
            if ( !IsDownloading )
            {// Loads it's info on-demand
                LoadScriptInfo ( e.Node.Text );
                btnDL.Enabled = true;
            }
        }

        // Loads a script's info to the labels
        private void LoadScriptInfo ( String file )
        {
            try
            {
                si = ScriptLoader.ScriptLoader.LoadScriptInfo ( file );

                lblName.Text = si.Name;
                lblHomepage.Text = si.Homepage;
                lblFormat.Text = si.URLFormat.ToString ( );
                lblAuthor.Text = si.Author;
                lblVersion.Text = si.Version;
            }
            catch ( Exception ex )
            {
                ErrorMessage ( ex.ToString ( ) );
            }
        }

        // Called when the "Download" button is clicked
        private void button1_Click ( Object sender, EventArgs e )
        {
            // If the URL is not valid
            if ( !si.URLFormat.IsMatch ( txtUrl.Text ) )
            {
                // Show an error message and exits.
                ErrorMessage ( "Invalid URL provided." );
                return;
            }

            // Creates a new scriptrunner instance and sets the event handlers
            var script = new ScriptRunner ( si.Path );
            script.Messaged += Script_Messaged;
            script.Errored += Script_Errored;
            script.Finished += Script_Finished;

            // Runs the script
            var thread = script.RunScript ( txtUrl.Text );

            IsDownloading = true;
            btnDL.Enabled = false;
            btnDL.UseWaitCursor = true;
        }

        // Called when log() is used
        private void Script_Messaged ( Object sender, String msg )
        {
            txtLog.InvokeEx ( log =>
            {
                if ( log.Disposing || log.IsDisposed )
                    return;
                log.AppendText ( $">{msg}{Environment.NewLine}" );
                log.SelectionStart = log.TextLength;
                log.ScrollToCaret ( );
            } );
        }

        // Called when an error/exception occurs
        private void Script_Errored ( Object sender, String err )
        {
            txtLog.InvokeEx ( log =>
            {
                if ( log.Disposing || log.IsDisposed )
                    return;

                var start = log.TextLength;
                Script_Messaged ( sender, err );
                var end = log.TextLength;

                log.Select ( start, end - start );
                log.SelectionColor = System.Drawing.Color.Red;

                log.Select ( end, end );
                log.SelectionColor = System.Drawing.Color.White;
            } );
        }

        // Called when the script is finished
        private void Script_Finished ( Object sender )
        {
            IsDownloading = false;
            btnDL.InvokeEx ( btn => {
                btn.Enabled = true;
                btn.UseWaitCursor = false;
            } );
            Script_Messaged ( sender, "Done." );
        }

        /// <summary>
        /// Shows a message box with an error message
        /// </summary>
        /// <param name="Message">The error message</param>
        private void ErrorMessage ( String Message )
        {
            MessageBox.Show ( Message, "Scripted Downloader | Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly );
        }

        // Shows the settings window
        private void settingsToolStripMenuItem_Click ( Object sender, EventArgs e )
        {
            using ( var settingsForm = new SettingsForm ( ) )
            {
                settingsForm.ShowDialog ( );
            }
        }

        private void aboutToolStripMenuItem_Click ( Object sender, EventArgs e )
        {
            using ( var about = new AboutForm ( ) )
            {
                about.ShowDialog ( );
            }
        }
    }

    // Class to make invoking easier (Source: http://stackoverflow.com/a/711419/2671392)
    public static class ISynchronizeInvokeExtensions
    {
        public static void InvokeEx<T> ( this T @this, Action<T> action ) where T : ISynchronizeInvoke
        {
            if ( @this.InvokeRequired )
            {
                try
                {
                    @this.Invoke ( action, new object[] { @this } );
                }
                catch ( Exception )
                {
                    throw;
                }
            }
            else
            {
                action?.Invoke ( @this );
            }
        }
    }
}