using System;
using System.Windows.Forms;

namespace ServerPL
{
    public partial class HelpForm2 : Form
    {
        private HelpForm3 frm3;
        private HelpForm4 frm4;
        private HelpForm5 frm5;

        public HelpForm2()
        {
            InitializeComponent();

            frm3 = new HelpForm3();
            frm4 = new HelpForm4();
            frm5 = new HelpForm5();
        }

        private void btnMessages_Click(object sender, EventArgs e)
        {
            frm3.SetData("", "", "", "", null);
            frm3.ShowDialog();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            frm4.SetData("", "", null);
            frm4.ShowDialog();
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            frm5.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
