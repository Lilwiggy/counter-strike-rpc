using System;
using System.IO;
using System.Windows.Forms;

namespace Installer
{
    public partial class Installer : Form
    {
        bool Failed;
        public Installer()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = Browse.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    File.Move("./FIles/gamestate_integration_discordpresence.cfg", Browse.SelectedPath + @"steamapps\common\Counter-Strike Global Offensive\csgo\cfg");
                }
                catch (IOException)
                {
                    Failed = true;
                }
                if (!Failed)
                    MessageBox.Show("Installed!");
                else
                    MessageBox.Show("Nice it's already installed! So now you have TWO copies! But that would be stupid so I just didn't do anything.");
            }
        }
    }
}
