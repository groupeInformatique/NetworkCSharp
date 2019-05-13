using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Network
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void editionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formRouteur frmR = new formRouteur();
            frmR.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            /*DialogResult m = MessageBox.Show("","Do you want to exit ?", MessageBoxButtons.OKCancel);

            if (m == DialogResult.Cancel)
            {

            }
            else if (m == DialogResult.OK)
            {
                Application.Exit();
            }*/

        }

        private void interfacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormIface frmI = new FormIface();
            frmI.Show();
        }

        private void sniffToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void speedToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
