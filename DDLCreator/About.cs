using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDLCreator
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            logo.Image = new Bitmap(Properties.Resources.WWD_Logo);
        }

        private void website_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.windworksdesign.com/");
            }
            catch
            {
                MessageBox.Show("Couldn't start your browser, please enter the link into your address bar.", "oops...", MessageBoxButtons.OK);
            }
        }
    }
}