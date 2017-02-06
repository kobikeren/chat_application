using System;
using System.Windows.Forms;

namespace ServerPL
{
    public partial class HelpForm1 : Form
    {
        public HelpForm1()
        {
            InitializeComponent();
        }

        private void HelpForm_Activated(object sender, EventArgs e)
        {
            txtIPA.Focus();
            txtIPA.SelectAll();
        }

        public void SetData(string ipaddress1, int port1)
        {
            txtIPA.Text = ipaddress1;
            txtPort.Text = port1.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtIPA.Text == "")
            {
                MessageBox.Show("please enter ip address");
                txtIPA.Focus();
                return;
            }
            int x = 0;
            if (!(int.TryParse(txtPort.Text, out x)))
            {
                MessageBox.Show("please enter port number");
                txtPort.Focus();
                txtPort.SelectAll();
                return;
            }
            //the information in the text boxes is valid.
            //insert that information to the properties
            myIPAddress = txtIPA.Text;
            myPort = int.Parse(txtPort.Text);
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        //the two properties of this form are: ip address, port
        private string myIPAddress;
        public string MyIPAddress
        {
            get { return myIPAddress; }
        }
        private int myPort;
        public int MyPort
        {
            get { return myPort; }
        }                
    }
}
