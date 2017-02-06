using System;
using System.Windows.Forms;

namespace ClientPL
{
    public partial class HelpForm3 : Form
    {
        public HelpForm3()
        {
            InitializeComponent();
        }

        private void HelpForm3_Activated(object sender, EventArgs e)
        {
            txtFirstName.Focus();
        }

        public void SetData(string fname1, string lname1, string uname1)
        {
            txtFirstName.Text = fname1;
            txtLastName.Text = lname1;
            lblUserName.Text = uname1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtFirstName.Text == "")
            {
                MessageBox.Show("please enter first name");
                txtFirstName.Focus();
                return;
            }
            if (txtLastName.Text == "")
            {
                MessageBox.Show("please enter last name");
                txtLastName.Focus();
                return;
            }
            fullName = txtFirstName.Text + " " + txtLastName.Text;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private string fullName;
        public string FullName
        {
            get { return fullName; }
        }
    }
}
