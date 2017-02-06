using System;
using System.Windows.Forms;

namespace ClientPL
{
    public partial class HelpForm2 : Form
    {
        public HelpForm2()
        {
            InitializeComponent();
        }

        private void HelpForm2_Activated(object sender, EventArgs e)
        {
            txtNamePrivate.Focus();
        }

        public void SetData(string namePrivate1)
        {
            txtNamePrivate.Text = namePrivate1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtNamePrivate.Text == "")
            {
                MessageBox.Show("please enter name to send the message as private");
                txtNamePrivate.Focus();
                return;
            }
            //the information in the text box is valid. insert that
            //information to the property
            namePrivate = txtNamePrivate.Text;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        //NamePrivate is the property of this form
        private string namePrivate;
        public string NamePrivate
        {
            get { return namePrivate; }
        }                        
    }
}
